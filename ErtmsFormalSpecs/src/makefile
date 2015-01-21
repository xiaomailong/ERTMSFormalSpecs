.phony:all clean increase_release_number release
.default:all

include etc/makefile.shared

XMLBOOSTER_ROOT=$(ERTMS_TOOLS_ROOT)/xmlb_2_29_2
XMLB_CALL=$(XMLBOOSTER_ROOT)/bin/xmlbgui.exe -CFG=../../../tools/xmlb_2_29_2/xmlbcfg.xml

all: 
	@make -C XmlBooster all 
	@make -C Utils all 
	@make -C DataDictionary all 
	@make -C Reports all 
	@make -C Importers all 
	@make -C GUI all 
	@make -C EFSTester all

clean:
	@make -C XmlBooster clean 
	@make -C Utils clean
	@make -C DataDictionary clean 
	@make -C Reports clean 
	@make -C Importers clean 
	@make -C GUI clean 
	@make -C EFSTester clean 

increase_release_number :
	@echo === Increase build number
	@-$(XMLB_CALL) -SCRIPT update_release_number.rcs GUI/Properties/AssemblyInfo.cs > /dev/null

release: clean increase_release_number Installer/EFSW_Installer_Script.iss	
	@echo === Rebuilding solution in Release mode
	$(BUILD) /t:Rebuild /p:Configuration=Release
	@echo === Building Installer package	
	@../depends/InnoInstaller5/ISCC.exe Installer/EFSW_Installer_Script.iss /q

