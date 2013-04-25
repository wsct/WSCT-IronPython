import clr
from WSCT.Helpers import *
from WSCT.Core import *
from WSCT.Wrapper import *

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
		self.context.establish()
		self.context.listReaderGroups()
		self.context.listReaders(self.context.groups[0])
		
	def terminateContext(self):
		self.contextObserver.unsubscribe()
		self.context.release()

	def useReader(self, readerName):
		print "iPy >> Reader to use:", readerName
		self.channel = CardChannel(self.context, readerName)
		self.channelObserver = ChannelObserverDemo(self.channel)

		if self.channel.connect(ShareMode.SCARD_SHARE_SHARED, Protocol.SCARD_PROTOCOL_ANY) == ErrorCode.SCARD_S_SUCCESS:
			pass

class ContextObserverDemo:
	def __init__(self, context):
		self.context = context
		self.subscribe()

	def subscribe(self):
		self.context.afterEstablishEvent += self.notifyEstablish
		self.context.afterGetStatusChangeEvent += self.notifyGetStatusChange
		self.context.afterListReaderGroupsEvent += self.notifyListReaderGroups
		self.context.afterListReadersEvent += self.notifyListReaders
		self.context.afterReleaseEvent += self.notifyRelease

	def unsubscribe(self):
		self.context.afterEstablishEvent -= self.notifyEstablish
		self.context.afterGetStatusChangeEvent -= self.notifyGetStatusChange
		self.context.afterListReaderGroupsEvent -= self.notifyListReaderGroups
		self.context.afterListReadersEvent -= self.notifyListReaders
		self.context.afterReleaseEvent -= self.notifyRelease

	def notifyEstablish(self, context, errorCode):
		print "iPy >> establish(): %s" % errorCode

	def notifyListReaders(self, context, group, errorCode):
		print "iPy >> listReaders(): %s" % errorCode
		print "       Readers found: %i" % context.readers.Count
		for reader in context.readers:
			print "       Reader: %s" % reader

	def notifyGetStatusChange(self, context, timeout, readerStates, errorCode):
		print "iPy >> getStatusChange(): %s" % errorCode

	def notifyListReaderGroups(self, context, errorCode):
		print "iPy >> listReaderGroups(): %s" % errorCode
		print "       Groups found: %i" % context.groups.Count

	def notifyRelease(self, context, errorCode):
		print "iPy >> release(): %s" % errorCode

class ChannelObserverDemo:
	def __init__(self, channel):
		self.channel = channel
		self.subscribe()

	def subscribe(self):
		self.channel.afterConnectEvent += self.notifyConnect;
		self.channel.afterDisconnectEvent += self.notifyDisconnect;
		self.channel.afterGetAttribEvent += self.notifyGetAttrib;
		self.channel.afterReconnectEvent += self.notifyReconnect;
		self.channel.afterTransmitEvent += self.notifyTransmit;

	def unsubscribe(self):
		self.channel.afterConnectEvent -= self.notifyConnect;
		self.channel.afterDisconnectEvent -= self.notifyDisconnect;
		self.channel.afterGetAttribEvent -= self.notifyGetAttrib;
		self.channel.afterReconnectEvent -= self.notifyReconnect;
		self.channel.afterTransmitEvent -= self.notifyTransmit;

	def notifyConnect(self, channel, shareMode, preferedProtocol, errorCode):
		print "iPy >> connect(%s, %s): %s" % (shareMode, preferedProtocol, errorCode)

	def notifyDisconnect(self, channel, disposition, errorCode):
		print "iPy >> disconnect(%s): %s" % (disposition, errorCode)
	
	def notifyGetAttrib(self, channel, attrib, buffer, errorCode):
		print "iPy >> getAttrib(%s): %s" % (attrib, errorCode)

	def notifyReconnect(self, channel, shareMode, preferedProtocol, initialization, errorCode):
		print "iPy >> reconnect(%s, %s, %s): %s" % (shareMode, preferedProtocol, initialization, errorCode)

	def notifyTransmit(self, channel, cardCommand, cardResponse, errorCode):
		print "iPy >> transmit(%s): %s" % (cardCommand, errorCode)
