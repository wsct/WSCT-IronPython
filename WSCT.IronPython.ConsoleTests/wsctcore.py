import clr
from WSCT.Helpers import *
from WSCT.Core import *
from WSCT.Core.Events import *
from WSCT.Wrapper import *
from WSCT.Wrapper.Desktop.Core import *
from System import EventHandler, EventArgs

def wsct_entry():
	print "iPy >> Entry point"
	demo = WSCTCoreDemo()
	demo.initializeContext()
	demo.terminateContext()

class WSCTCoreDemo:
	def __init__(self):
		pass

	def initializeContext(self):
		self.context = CardContext()
		self.contextObserver = ContextObserverDemo(self.context)
		self.context.Establish()
		self.context.ListReaderGroups()
		self.context.ListReaders(self.context.Groups[0])
		
	def terminateContext(self):
		self.contextObserver.unsubscribe()
		self.context.Release()

	def useReader(self, readerName):
		print "iPy >> Reader to use:", readerName
		self.channel = CardChannel(self.context, readerName)
		self.channelObserver = ChannelObserverDemo(self.channel)

		if self.channel.Connect(ShareMode.Shared, Protocol.Any) == ErrorCode.Success:
			pass

class ContextObserverDemo:
	def __init__(self, context):
		self.context = context
		self.subscribe()

	def subscribe(self):
		self.context.AfterEstablishEvent += self.notifyEstablish
		self.context.AfterGetStatusChangeEvent += self.notifyGetStatusChange
		self.context.AfterListReaderGroupsEvent += self.notifyListReaderGroups
		self.context.AfterListReadersEvent += self.notifyListReaders
		self.context.AfterReleaseEvent += self.notifyRelease

	def unsubscribe(self):
		self.context.AfterEstablishEvent -= self.notifyEstablish
		self.context.AfterGetStatusChangeEvent -= self.notifyGetStatusChange
		self.context.AfterListReaderGroupsEvent -= self.notifyListReaderGroups
		self.context.AfterListReadersEvent -= self.notifyListReaders
		self.context.AfterReleaseEvent -= self.notifyRelease

	def notifyEstablish(self, sender, eventArgs):
		print "iPy >> Establish(): %s" % eventArgs.ReturnValue

	def notifyListReaders(self, sender, eventArgs):
		print "iPy >> listReaders(): %s" % eventArgs.ReturnValue
		print "       Readers found: %i" % sender.Readers.Count
		for reader in sender.Readers:
			print "       Reader: %s" % reader

	def notifyGetStatusChange(self, sender, eventArgs):
		print "iPy >> GetStatusChange(): %s" % eventArgs.ReturnValue

	def notifyListReaderGroups(self, sender, eventArgs):
		print "iPy >> ListReaderGroups(): %s" % eventArgs.ReturnValue
		print "       Groups found: %i" % sender.Groups.Count

	def notifyRelease(self, sender, eventArgs):
		print "iPy >> Release(): %s" % eventArgs.ReturnValue

class ChannelObserverDemo:
	def __init__(self, channel):
		self.channel = channel
		self.subscribe()

	def subscribe(self):
		self.channel.AfterConnectEvent += self.notifyConnect;
		self.channel.AfterDisconnectEvent += self.notifyDisconnect;
		self.channel.AfterGetAttribEvent += self.notifyGetAttrib;
		self.channel.AfterReconnectEvent += self.notifyReconnect;
		self.channel.AfterTransmitEvent += self.notifyTransmit;

	def unsubscribe(self):
		self.channel.AfterConnectEvent -= self.notifyConnect;
		self.channel.AfterDisconnectEvent -= self.notifyDisconnect;
		self.channel.AfterGetAttribEvent -= self.notifyGetAttrib;
		self.channel.AfterReconnectEvent -= self.notifyReconnect;
		self.channel.AfterTransmitEvent -= self.notifyTransmit;

	def notifyConnect(self, sender, eventArgs):
		print "iPy >> connect(%s, %s): %s" % (eventArgs.ShareMode, eventArgs.PreferedProtocol, eventArgs.ReturnValue)

	def notifyDisconnect(self, sender, eventArgs):
		print "iPy >> disconnect(%s): %s" % (eventArgs.Disposition, eventArgs.ReturnValue)
	
	def notifyGetAttrib(self, sender, eventArgs):
		print "iPy >> getAttrib(%s): %s" % (eventArgs.Attrib, eventArgs.ReturnValue)

	def notifyReconnect(self, sender, eventArgs):
		print "iPy >> reconnect(%s, %s, %s): %s" % (eventArgs.ShareMode, eventArgs.PreferedProtocol, eventArgs.Initialization, eventArgs.ReturnValue)

	def notifyTransmit(self, sender, eventArgs):
		print "iPy >> transmit(%s): %s" % (eventArgs.Command, eventArgs.ReturnValue)
