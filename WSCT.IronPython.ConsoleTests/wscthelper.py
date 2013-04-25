import clr
from WSCT.Helpers import *

class ArrayOfBytesDemo:
	def __init__(self, stringData):
		self.stringData = stringData

	def doIt(self):
		print self.stringData

		bytesArray = ArrayOfBytes.fromString(self.stringData)
		print bytesArray

		hexaString = ArrayOfBytes.toHexa(bytesArray)
		print hexaString
