.phony: Comparer

META=DataDictionary
ADDITIONAL_BUILD_TARGET=src/Comparer.cs

include ../etc/makefile.shared
include ../etc/makefile.xmlb


Comparer: src/Comparer.cs

src/Comparer.cs: $(META).xmlb 
	$(XMLB_CALL) -SCRIPT generate_comparer.rcs $<


