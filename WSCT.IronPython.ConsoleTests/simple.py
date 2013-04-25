import sys

def Simple():
    print "I'm in simple.py";

class MyClass:
    def __init__(self):
        pass
               
    def somemethod(self):
        print 'simple.py: in some method'
               
    def isodd(self, n):
        return 1 == n % 2

    def calldir(self):
        print dir()
        print sys.path