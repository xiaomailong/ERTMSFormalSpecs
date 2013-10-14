.phony: Comparer

META=DataDictionary

include ../etc/makefile.shared
include ../etc/makefile.xmlb

Comparer: src/Comparer.cs

src/Comparer.cs: $(META).xmlb 
	$(XMLB_CALL) -SCRIPT generate_comparer.rcs $<


