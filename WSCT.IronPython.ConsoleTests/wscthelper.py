import clr
from WSCT.Helpers import *

class ArrayOfBytesDemo:
	def __init__(self, stringData):
		self.stringData = stringData

	def doIt(self):
		print self.stringData

		bytesArray = BytesHelper.FromString(self.stringData)
		print bytesArray

		hexaString = BytesHelper.ToHexa(bytesArray)
		print hexaString
