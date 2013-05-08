WSCT-IronPython
===============

Public repository for WSCT IronPython project.

Why this project ?
------------------
It's a demonstration of the use of *Python scripts* with WSCT projects.

It's based on [IronPython](http://www.ironpython.net/) (an open-source implementation of the Python programming language which is tightly integrated with the .NET Framework). Python script is able to fully access .net object while .net code is able to interact with python objects.


Prerequisites
-------------
* Installation of IronPython binaries ([http://www.ironpython.net/]())
* Download of WSCT-Core project


Command line script runner
--------------------------
Python script needs to publish a function named `wsct_entry` that will be called from `WSCT.IronPython.Execute` to launch the script:

```
def wsct_entry():
pass
```

The command line program can be run:

* followed by python script file name and an XML configuration file used to insert some .net libraries for avaibility in Python script:

  ```
  WSCT.IronPython.Execute wsct_entry.py wsct_entry.xml
  ```

* followed only by the python script file name (`wsct_entry.xml` is used by default as configuration and needs to be there)

  ```
  WSCT.IronPython.Execute script.py
  ```

* without any parameter (`wsct_entry.py` and `wsct_entry.xml` are used by default and need to be there)

  ```
  WSCT.IronPython.Execute script.py
  ```
