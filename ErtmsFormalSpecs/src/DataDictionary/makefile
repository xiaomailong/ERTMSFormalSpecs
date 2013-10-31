.phony: Comparer

META=DataDictionary
ADDITIONAL_BUILD_TARGET=src/Compare/Comparer.cs

include ../etc/makefile.shared
include ../etc/makefile.xmlb


Comparer: src/Compare/Comparer.cs

src/Compare/Comparer.cs: $(META).xmlb 
	$(XMLB_CALL) -SCRIPT generate_comparer.rcs $<


