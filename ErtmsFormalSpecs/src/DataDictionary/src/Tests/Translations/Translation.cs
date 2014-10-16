// ------------------------------------------------------------------------------
// -- Copyright ERTMS Solutions
// -- Licensed under the EUPL V.1.1
// -- http://joinup.ec.europa.eu/software/page/eupl/licence-eupl
// --
// -- This file is part of ERTMSFormalSpec software and documentation
// --
// --  ERTMSFormalSpec is free software: you can redistribute it and/or modify
// --  it under the terms of the EUPL General Public License, v.1.1
// --
// -- ERTMSFormalSpec is distributed in the hope that it will be useful,
// -- but WITHOUT ANY WARRANTY; without even the implied warranty of
// -- MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// --
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace DataDictionary.Tests.Translations
{
    public class Translation : Generated.Translation, ICommentable, TextualExplain
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Translation()
            : base()
        {
        }

        /// <summary>
        /// Provides the name of the translation
        /// </summary>
        public override string Name
        {
            get
            {
                string retVal = base.Name;

                if (Utils.Utils.isEmpty(retVal) && countSourceTexts() > 0)
                {
                    retVal = getSourceTexts(0).Name;
                }

                return retVal;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <summary>
        /// Provides the source texts for this dictionary
        /// </summary>
        public System.Collections.ArrayList SourceTexts
        {
            get
            {
                if (allSourceTexts() == null)
                {
                    setAllSourceTexts(new System.Collections.ArrayList());
                }
                return allSourceTexts();
            }
            set
            {
                setAllSourceTexts(value);
            }
        }

        /// <summary>
        /// Provides the sub-steps for this dictionary
        /// </summary>
        public System.Collections.ArrayList SubSteps
        {
            get
            {
                if (allSubSteps() == null)
                {
                    setAllSubSteps(new System.Collections.ArrayList());
                }
                return allSubSteps();
            }
            set
            {
                setAllSubSteps(value);
            }
        }

        /// <summary>
        /// The explanation of this translation, as RTF pseudo code
        /// </summary>
        /// <returns></returns>
        public override string getExplain()
        {
            string retVal = "";

            string indent = "";

            if (SourceTexts.Count > 0)
            {
                if (SourceTexts.Count == 1)
                {
                    retVal = retVal + "Source text\n";
                }
                else
                {
                    retVal = retVal + "Source texts\n";
                }
                indent = "  ";
                foreach (SourceText text in SourceTexts)
                {
                    retVal = retVal + indent + text.Name + "\n";
                }
                indent = "";
                if (SourceTexts.Count == 1)
                {
                    retVal = retVal + "Is translated as\n";
                }
                else
                {
                    retVal = retVal + "Are translated as\n";
                }
            }

            foreach (SubStep subStep in SubSteps)
            {
                retVal = retVal + subStep.getExplain();
            }

            return retVal;
        }

        /// <summary>
        /// The enclosing translation dictionary
        /// </summary>
        public Tests.Translations.TranslationDictionary TranslationDictionary
        {
            get { return Enclosing as Tests.Translations.TranslationDictionary; }
        }

        /// <summary>
        /// Provides the enclosing collection
        /// </summary>
        public override System.Collections.ArrayList EnclosingCollection
        {
            get
            {
                System.Collections.ArrayList retVal = null;
                Tests.Translations.TranslationDictionary dictionary = Enclosing as Tests.Translations.TranslationDictionary;
                if (dictionary != null)
                {
                    retVal = dictionary.Translations;
                }
                else
                {
                    Tests.Translations.Folder folder = Enclosing as Tests.Translations.Folder;
                    if (folder != null)
                    {
                        retVal = folder.Translations;
                    }
                }
                return retVal;
            }
        }


        /// <summary>
        /// Updates the step according to this translation
        /// </summary>
        /// <param name="step"></param>
        public void UpdateStep(Step step)
        {
            step.Requirements.Clear();
            foreach (ReqRef reqRef in Requirements)
            {
                step.appendRequirements((ReqRef)reqRef.Duplicate());
            }

            int subStepCounter = 1;
            foreach (SubStep subStep in SubSteps)
            {
                SubStep newSubStep = (SubStep)Generated.acceptor.getFactory().createSubStep();
                newSubStep.Name = "Sub-step" + subStepCounter;
                newSubStep.setSkipEngine(subStep.getSkipEngine());
                step.appendSubSteps(newSubStep);

                foreach (Rules.Action action in subStep.Actions)
                {
                    Rules.Action newAct = (Rules.Action) action.Duplicate();
                    newSubStep.appendActions(newAct);
                    Review(newAct);
                }

                foreach (Expectation expectation in subStep.Expectations)
                {
                    Expectation newExp = (Expectation)expectation.Duplicate();
                    newSubStep.appendExpectations(newExp);
                    Review(newExp);
                }

                subStepCounter++;
            }
        }


        /// <summary>
        /// Explains the Translation with indentation
        /// </summary>
        /// <returns></returns>
        public string getExplain(bool explainSubElements)
        {
            return getExplain(explainSubElements, 0);
        }

        /// <summary>
        /// Explains the Translation with indentation
        /// </summary>
        /// <returns></returns>
        public string getExplain(bool explainSubElements, int indent)
        {
            string result = "";

            // The source textes for this translation
            if (SourceTexts.Count > 1)
            {
                int i = 1;
                result += TextualExplainUtilities.Pad("{\\b SOURCE TEXTS}\\par", indent);
                foreach (SourceText sourceText in SourceTexts)
                {
                    result += TextualExplainUtilities.Pad("{\\b SOURCE TEXT "+i+"}\\par", indent);
                    result += sourceText.getExplain(explainSubElements, indent + 2) + "\\par";
                    i += 1;
                }
            }
            else
            {
                result += TextualExplainUtilities.Pad("{\\b SOURCE TEXT}\\par", indent);
                foreach (SourceText sourceText in SourceTexts)
                {
                    result += sourceText.getExplain(explainSubElements, indent + 2) + "\\par";
                }
            }

            // The translation itself
            if (explainSubElements)
            {
                result += TextualExplainUtilities.Pad("\\par{\\b TRANSLATION }\\par", indent);

                foreach (SubStep subStep in SubSteps)
                {
                    result += subStep.getExplain(indent + 2, explainSubElements) + "\\par";
                }
            }

            result += "\\par";

            return result;
        }

        public string getSourceTextExplain()
        {
            string result = "";
            string prefix = "";
            int textCount = 1;

            foreach (SourceText sourceText in SourceTexts)
            {
                if (SourceTexts.Count > 1)
                {
                    prefix = textCount + ". ";
                }
                else
                {
                    prefix = "";
                }
                result += prefix + sourceText.Name + "\n";
                textCount++;
            }

            return result;
        }

        /// <summary>
        /// Review the expressions associated to this expectation
        /// </summary>
        /// <param name="expectation"></param>
        private void Review(Expectation expectation)
        {
            expectation.Value = ReviewExpression(expectation.Step, expectation.Value);
        }

        /// <summary>
        /// Review the expressions associated to this action
        /// </summary>
        /// <param name="action"></param>
        private void Review(Rules.Action action)
        {
            try
            {
                action.ExpressionText = ReviewExpression(action.Step, action.ExpressionText);
            }
            catch (Exception e)
            {
                action.AddException(e);
            }
        }

        /// <summary>
        /// Updates an expression according to translation rules
        /// </summary>
        /// <param name="step">the step in which the expression occurs</param>
        /// <param name="expression"></param>
        /// <returns>the updated string</returns>
        private string ReviewExpression(Step step, string expression)
        {
            string retVal = expression;

            if (expression.IndexOf('%') >= 0)
            {
                SubSequence subSequence = step.TestCase.SubSequence;

                retVal = retVal.Replace("%D_LRBG", format_decimal_as_str(subSequence.getD_LRBG()));
                retVal = retVal.Replace("%Level", format_level(subSequence.getLevel()));
                retVal = retVal.Replace("%Mode", format_mode(subSequence.getMode()));
                retVal = retVal.Replace("%NID_LRBG", format_decimal_as_str(subSequence.getNID_LRBG()));
                retVal = retVal.Replace("%Q_DIRLRBG", format_decimal_as_str(subSequence.getQ_DIRLRBG()));
                retVal = retVal.Replace("%Q_DIRTRAIN", format_decimal_as_str(subSequence.getQ_DIRTRAIN()));
                retVal = retVal.Replace("%Q_DLRBG", format_decimal_as_str(subSequence.getQ_DLRBG()));
                retVal = retVal.Replace("%RBC_ID", format_decimal_as_str(subSequence.getRBC_ID()));
                retVal = retVal.Replace("%RBCPhone", format_str(subSequence.getRBCPhone()));

                retVal = retVal.Replace("%Step_Distance", step.getDistance() + "");
                retVal = retVal.Replace("%Step_LevelIN", format_level(step.getLevelIN()));
                retVal = retVal.Replace("%Step_LevelOUT", format_level(step.getLevelOUT()));
                retVal = retVal.Replace("%Step_ModeIN", format_mode(step.getModeOUT()));
                retVal = retVal.Replace("%Step_ModeOUT", format_mode(step.getModeOUT()));

                int max_step_messages = 8;
                for (int i = 0; i < max_step_messages; i++)
                {
                    if (step.StepMessages.Count > i)
                    {
                        DBElements.DBMessage message = step.StepMessages[i] as DBElements.DBMessage;
                        if (message != null)
                        {
                            retVal = retVal.Replace("%Step_Messages_" + i, format_message(message));
                        }
                    }
                    else
                    {
                        retVal = retVal.Replace("%Step_Messages_" + i, format_default_message(expression));
                    }
                }

                if (retVal.IndexOf("%") > 0)
                {
                    step.AddError("Cannot completely translate this step");
                }
            }

            return retVal;
        }

        /// <summary>
        /// Takes the string provided and returns the corresponding Mode enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string format_mode(string str)
        {
            string retVal = "";

            str = format_str(str);
            int val = format_decimal(str);
            if (val != -1)
            {
                switch (val)
                {
                    case 6:
                        retVal = "Mode.SB";
                        break;
                    default:
                        retVal = "Mode.Unknown";
                        break;
                }
            }
            else
            {
                retVal = "Mode." + str;
            }

            return retVal;
        }

        /// <summary>
        /// Takes the enum provided and returns the corresponding Mode enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string format_mode(Generated.acceptor.ST_MODE mode)
        {
            string retVal = "";

            switch (mode)
            {
                case Generated.acceptor.ST_MODE.Mode_FS:
                    retVal = "Mode.FS";
                    break;
                case Generated.acceptor.ST_MODE.Mode_IS:
                    retVal = "Mode.IS";
                    break;
                case Generated.acceptor.ST_MODE.Mode_LS:
                    retVal = "Mode.LS";
                    break;
                case Generated.acceptor.ST_MODE.Mode_NA:
                    retVal = "Mode.NA";
                    break;
                case Generated.acceptor.ST_MODE.Mode_NL:
                    retVal = "Mode.NL";
                    break;
                case Generated.acceptor.ST_MODE.Mode_NP:
                    retVal = "Mode.NP";
                    break;
                case Generated.acceptor.ST_MODE.Mode_OS:
                    retVal = "Mode.OS";
                    break;
                case Generated.acceptor.ST_MODE.Mode_PSH:
                    retVal = "Mode.PSH";
                    break;
                case Generated.acceptor.ST_MODE.Mode_PT:
                    retVal = "Mode.PT";
                    break;
                case Generated.acceptor.ST_MODE.Mode_RE:
                    retVal = "Mode.RE";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SB:
                    retVal = "Mode.SB";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SF:
                    retVal = "Mode.SF";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SH:
                    retVal = "Mode.SH";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SL:
                    retVal = "Mode.SL";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SN:
                    retVal = "Mode.SN";
                    break;
                case Generated.acceptor.ST_MODE.Mode_SR:
                    retVal = "Mode.SR";
                    break;
                case Generated.acceptor.ST_MODE.Mode_TR:
                    retVal = "Mode.TR";
                    break;
                case Generated.acceptor.ST_MODE.Mode_UF:
                    retVal = "Mode.UF";
                    break;
                default:
                    retVal = "Mode.Unknown";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Takes the message provided and returns the corresponding message value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string format_message(DBElements.DBMessage message)
        {
            string retVal = "";
            switch (message.MessageType)
            {
                case Generated.acceptor.DBMessageType.aEUROBALISE:
                    retVal = format_eurobalise_message(message);
                    break;
                case Generated.acceptor.DBMessageType.aEUROLOOP:
                    retVal = format_euroloop_message(message);
                    break;
                case Generated.acceptor.DBMessageType.aEURORADIO:
                    retVal = format_euroradio_message(message);
                    break;
            }
            return retVal;
        }


        public string format_default_message(string expression)
        {
            string retVal = "<not a structure type>";

            int index = expression.IndexOf("<-");
            if ( index > 0 )
            {
                string variableText = expression.Substring(0, index).Trim();
                Interpreter.Expression expressionTree = EFSSystem.Parser.Expression(Dictionary, variableText);
                if (expressionTree != null)
                {
                    Types.Structure structureType = expressionTree.GetExpressionType() as Types.Structure;
                    retVal = structureType.DefaultValue.LiteralName;
                }
            }
            
            return retVal;
        }

        private static string format_eurobalise_message(DBElements.DBMessage message)
        {
            EFSSystem system = EFSSystem.INSTANCE;

            DataDictionary.Types.NameSpace nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(system.Dictionaries[0], "Messages.EUROBALISE");
            Types.Structure structureType = (Types.Structure)system.findType(nameSpace, "Message");
            Values.StructureValue structure = new Values.StructureValue(structureType);

            int currentIndex = 0;
            FillStructure(nameSpace, message.Fields, ref currentIndex, structure); // fills the message fields


            // then we fill the packets
            Variables.IVariable subSequenceVariable;
            if (structure.SubVariables.TryGetValue("Sequence1", out subSequenceVariable))
            {
                subSequenceVariable.Value = get_message_packets(message, nameSpace, system);
            }
            else
            {
                throw new Exception("Cannot find SubSequence in variable");
            }

            return structure.Name;
        }



        static Values.ListValue get_message_packets(DBElements.DBMessage message, DataDictionary.Types.NameSpace nameSpace, EFSSystem system)
        {
            Values.ListValue retVal;

            Types.Collection collectionType = (Types.Collection)system.findType(nameSpace, "Collection1");
            Types.Structure subStructure1Type = (Types.Structure)system.findType(nameSpace, "SubStructure1");

            string packetLocation = "Messages.PACKET.";
            if (nameSpace.FullName.Contains("TRAIN_TO_TRACK"))
            {
                packetLocation += "TRAIN_TO_TRACK.Message";
            }
            else
            {
                packetLocation += "TRACK_TO_TRAIN.Message";
            }

            Types.Structure packetStructure = (Types.Structure)system.findType(nameSpace, packetLocation);

            retVal = new Values.ListValue(collectionType, new List<Values.IValue>());

            foreach (DBElements.DBPacket packet in message.Packets)
            {
                Tests.DBElements.DBField nidPacketField = packet.Fields[0] as Tests.DBElements.DBField;
                if (nidPacketField.Value != "255")  // 255 means "end of information"
                {
                    int packetId = int.Parse(nidPacketField.Value);
                    Values.StructureValue subStructure = FindStructure(packetId);

                    int currentIndex = 0;
                    FillStructure(nameSpace, packet.Fields, ref currentIndex, subStructure);
                    Values.StructureValue subStructure1 = new Values.StructureValue(subStructure1Type);
                    Values.StructureValue packetValue = new Values.StructureValue(packetStructure);
                    subStructure1.SubVariables.ElementAt(0).Value.Value = packetValue;
                    retVal.Val.Add(subStructure1);

                    // Find the right variable in the packet to add the structure we just created
                    foreach (KeyValuePair<string, Variables.IVariable> pair in packetValue.SubVariables)
                    {
                        string variableName = pair.Key;
                        if (subStructure.Structure.FullName.Contains(variableName))
                        {
                            Variables.IVariable variable = pair.Value;
                            variable.Value = subStructure;

                            break;
                        }
                    }
                }
            }

            return retVal;
        }


        /// <summary>
        /// Finds the type of the structure corresponding to the provided NID_PACKET
        /// </summary>
        /// <param name="nameSpace">The namespace where the type has to be found</param>
        /// <param name="nidPacket">The id of the packet</param>
        /// <returns></returns>
        private static Values.StructureValue FindStructure(int nidPacket)
        {
            EFSSystem system = EFSSystem.INSTANCE;
            Types.Structure structure = null;
            DataDictionary.Types.NameSpace nameSpace;

            if (nidPacket != 44)
            {
                nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(system.Dictionaries[0], "Messages.PACKET.TRACK_TO_TRAIN");
                foreach (DataDictionary.Types.NameSpace subNameSpace in nameSpace.NameSpaces)
                {
                    Types.Structure structureType = (Types.Structure)system.findType(subNameSpace, subNameSpace.FullName + ".Message");
                    Values.StructureValue structureValue = new Values.StructureValue(structureType);

                    foreach (KeyValuePair<string, Variables.IVariable> pair in structureValue.SubVariables)
                    {
                        string variableName = pair.Key;
                        if (variableName.Equals("NID_PACKET"))
                        {
                            Values.IntValue value = pair.Value.Value as Values.IntValue;
                            if (value.Val == nidPacket)
                            {
                                structure = structureType;
                            }
                        }
                        if (structure != null)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(system.Dictionaries[0], "Messages.PACKET.DATA_USED_BY_APPLICATIONS_OUTSIDE_THE_ERTMS_ETCS_SYSTEM");
                structure = (Types.Structure)system.findType(nameSpace, nameSpace.FullName + ".Message");
            }

            Values.StructureValue retVal = null;
            if (structure != null)
            {
                retVal = new Values.StructureValue(structure);
            }

            return retVal;
        }

        /// <summary>
        /// Fills the given structure with the values provided from the database
        /// </summary>
        /// <param name="aNameSpace">Namespace of the structure</param>
        /// <param name="fields">Fields to be copied into the structure</param>
        /// <param name="index">Index (of fields list) from which we have to start copying</param>
        /// <param name="aStructure">The structure to be filled</param>
        private static void FillStructure(Types.NameSpace aNameSpace, ArrayList fields, ref int currentIndex, Values.StructureValue aStructure)
        {
            EFSSystem system = EFSSystem.INSTANCE;

            int j = 0;
            while ( currentIndex < fields.Count )
            {
                Tests.DBElements.DBField field = fields[currentIndex] as Tests.DBElements.DBField;

                KeyValuePair<string, Variables.IVariable> pair = aStructure.SubVariables.ElementAt(j);
                Variables.IVariable variable = pair.Value;

                if (variable.Name.StartsWith(field.Variable))  // we use StartsWith and not Equals because we can have N_ITER_1 and N_ITER
                {
                    if (variable.Type is Types.Enum)
                    {
                        Types.Enum type = variable.Type as Types.Enum;
                        foreach (DataDictionary.Constants.EnumValue enumValue in type.Values)
                        {
                            int value = int.Parse(enumValue.getValue());
                            int other = int.Parse(field.Value);
                            if (value == other)
                            {
                                variable.Value = enumValue;
                                j++;
                                break;
                            }
                        }
                    }
                    else if (variable.Type is Types.Range)
                    {
                        Types.Range type = variable.Type as Types.Range;
                        decimal val = decimal.Parse(field.Value);
                        variable.Value = new Values.IntValue(type, val);
                        j++;
                    }
                    else if (variable.Type is Types.StringType)
                    {
                        Types.StringType type = variable.Type as Types.StringType;
                        variable.Value = new Values.StringValue(type, field.Value);
                        j++;

                    }
                    else
                    {
                        throw new Exception("Unhandled variable type");
                    }
                    if (field.Variable.Equals("N_ITER")) // we have to create a sequence
                    {
                        KeyValuePair<string, Variables.IVariable> sequencePair = aStructure.SubVariables.ElementAt(j);
                        Variables.IVariable sequenceVariable = sequencePair.Value;
                        Types.Collection collectionType = (Types.Collection)system.findType(aNameSpace, sequenceVariable.TypeName);
                        Values.ListValue sequence = new Values.ListValue(collectionType, new List<Values.IValue>());

                        int value = int.Parse(field.Value);
                        for (int k = 0; k < value; k++)
                        {
                            Types.Structure structureType = (Types.Structure)system.findType(aNameSpace, sequence.CollectionType.Type.FullName);
                            Values.StructureValue structureValue = new Values.StructureValue(structureType);
                            FillStructure(aNameSpace, fields, ref currentIndex, structureValue);
                            sequence.Val.Add(structureValue);
                        }
                        sequenceVariable.Value = sequence;
                        j++;
                    }
                }

                // if all the fields of the structue are filled, we terminated
                if (j == aStructure.SubVariables.Count)
                {
                    break;
                }
                else 
                {
                    currentIndex += 1;
                }
            }
        }


        private static string format_euroloop_message(DBElements.DBMessage message)
        {
            EFSSystem system = EFSSystem.INSTANCE;

            DataDictionary.Types.NameSpace nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(system.Dictionaries[0], "Messages.EUROLOOP");
            Types.Structure structureType = (Types.Structure)system.findType(nameSpace, "Message");
            Values.StructureValue structure = new Values.StructureValue(structureType);

            int currentIndex = 0;
            FillStructure(nameSpace, message.Fields, ref currentIndex, structure);


            // then we fill the packets
            Variables.IVariable subSequenceVariable;
            if (structure.SubVariables.TryGetValue("Sequence1", out subSequenceVariable))
            {
                  subSequenceVariable.Value = get_message_packets(message, nameSpace, system);
            }
            else
            {
                throw new Exception("Cannot find SubSequence in variable");
            }

            return structure.Name;
        }

        private static string format_euroradio_message(DBElements.DBMessage message)
        {
            EFSSystem system = EFSSystem.INSTANCE;

            DataDictionary.Types.NameSpace rbcRoot = OverallNameSpaceFinder.INSTANCE.findByName(system.Dictionaries[0], "Messages.MESSAGE");

            // Get the EFS namespace corresponding to the message 
            // Select the appropriate message type, tracktotrain or traintotrack
            Tests.DBElements.DBField nidMessage = message.Fields[0] as Tests.DBElements.DBField;
            string msg_id = get_namespace_from_ID( nidMessage.Value );

            DataDictionary.Types.NameSpace nameSpace = OverallNameSpaceFinder.INSTANCE.findByName(rbcRoot, msg_id);

            if (nameSpace == null)
            {
                throw new Exception("Message type not found in EFS");
            }

            // The EURORADIO messages are defined in the namespaces TRACK_TO_TRAIN and TRAIN_TO_TRACK, which enclose the specific message namespaces
            // So we get the message type from nameSpace.EnclosingNameSpace and the actual structure corresponding to the message in nameSpace
            Types.Structure enclosingStructureType = (Types.Structure)system.findType(nameSpace.EnclosingNameSpace, "Message");
            Values.StructureValue Message = new Values.StructureValue(enclosingStructureType);


            // Within the message, get the appropriate field and get that structure
            Types.Structure structureType = (Types.Structure)system.findType(nameSpace, "Message");
            Values.StructureValue structure = new Values.StructureValue(structureType);


            // Fill the structure
            int currentIndex = 0;
            FillStructure(nameSpace, message.Fields, ref currentIndex, structure);


            // and fill the packets
            Variables.IVariable subSequenceVariable;
            if (structure.SubVariables.TryGetValue("Sequence1", out subSequenceVariable)
                && message.Packets.Count > 0)
            {
                subSequenceVariable.Value = get_message_packets(message, nameSpace, system);
            }

            // Fill the correct field in Message with the structure.
            foreach (KeyValuePair<string, Variables.IVariable> pair in Message.SubVariables)
            {
                if (msg_id.EndsWith(pair.Key))
                {
                    pair.Value.Type = structureType;
                    pair.Value.Value = structure;
                }
            }

            return Message.Name;
        }


        /// <summary>
        /// Takes the string provided and returns the corresponding RBC message
        /// </summary>
        /// <param name="NID_MESSAGE"></param>
        /// <returns></returns>
        private static string get_namespace_from_ID(string str)
        {
            string retVal = "";
            if (format_decimal(str) < 100)
            {
                retVal = "TRACK_TO_TRAIN.";
            }
            else
            {
                retVal = "TRAIN_TO_TRACK.";
            }
            switch (format_decimal(str))
            {
                case 2:
                    retVal += "SR_AUTHORISATION";
                    break;
                case 3:
                    retVal += "MOVEMENT_AUTHORITY";
                    break;
                case 6:
                    retVal += "RECOGNITION_OF_EXIT_FROM_TRIP_MODE";
                    break;
                case 8:
                    retVal += "ACKNOWLEDGEMENT_OF_TRAIN_DATA";
                    break;
                case 9:
                    retVal += "REQUEST_TO_SHORTEN_MA";
                    break;
                case 15:
                    retVal += "CONDITIONAL_EMERGENCY_STOP";
                    break;
                case 16:
                    retVal += "UNCONDITIONAL_EMERGENCY_STOP";
                    break;
                case 18:
                    retVal += "REVOCATION_OF_EMERGENCY_STOP";
                    break;
                case 24:
                    retVal += "GENERAL_MESSAGE";
                    break;
                case 27:
                    retVal += "SH_REFUSED";
                    break;
                case 28:
                    retVal += "SH_AUTHORISED";
                    break;
                case 32:
                    retVal += "RBC.RIU_SYSTEM_VERSION";
                    break;
                case 33:
                    retVal += "MA_WITH_SHIFTED_LOCATION_REFERENCE";
                    break;
                case 34:
                    retVal += "TRACK_AHEAD_FREE_REQUEST";
                    break;
                case 37:
                    retVal += "INFILL_MA";
                    break;
                case 38:
                    retVal += "INITIATION_OF_A_COMMUNICATION_SESSION";
                    break;
                case 39:
                    retVal += "ACKNOWLEDGEMENT_OF_TERMINATION_OF_A_COMMUNICATION_SESSION";
                    break;
                case 40:
                    retVal += "TRAIN_REJECTED";
                    break;
                case 41:
                    retVal += "TRAIN_ACCEPTED";
                    break;
                case 43:
                    retVal += "SOM_POSITION_REPORT_CONFIRMED_BY_RBC";
                    break;
                case 45:
                    retVal += "ASSIGNMENT_OF_COORDINATE_SYSTEM";
                    break;
                case 129:
                    retVal += "VALIDATED_TRAIN_DATA";
                    break;
                case 130:
                    retVal += "REQUEST_FOR_SHUNTING";
                    break;
                case 132:
                    retVal += "MA_REQUEST";
                    break;
                case 136:
                    retVal += "TRAIN_POSITION_REPORT";
                    break;
                case 137:
                    retVal += "REQUEST_TO_SHORTEN_MA_IS_GRANTED";
                    break;
                case 138:
                    retVal += "REQUEST_TO_SHORTEN_MA_IS_REJECTED";
                    break;
                case 146:
                    retVal += "ACKNOWLEDGEMENT";
                    break;
                case 147:
                    retVal += "ACKNOWLEDGEMENT_OF_EMERGENCY_STOP";
                    break;
                case 149:
                    retVal += "TRACK_AHEAD_FREE_GRANTED";
                    break;
                case 150:
                    retVal += "END_OF_MISSION";
                    break;
                case 153:
                    retVal += "RADIO_INFILL_REQUEST";
                    break;
                case 154:
                    retVal += "NO_COMPATIBLE_VERSION_SUPPORTED";
                    break;
                case 155:
                    retVal += "INITIATION_OF_A_COMMUNICATION_SESSION";
                    break;
                case 156:
                    retVal += "TERMINATION_OF_A_COMMUNICATION_SESSION";
                    break;
                case 157:
                    retVal += "SOM_POSITION_REPORT";
                    break;
                case 158:
                    retVal += "TEXT_MESSAGE_ACKNOWLEDGED_BY_DRIVER";
                    break;
                case 159:
                    retVal += "SESSION_ESTABLISHED";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Takes the string provided and returns the corresponding Level enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string format_level(string str)
        {
            string retVal = "";

            switch (format_decimal(str))
            {
                case 0:
                    retVal = "Level.L0";
                    break;
                case 1:
                    retVal = "Level.L1";
                    break;
                case 2:
                    retVal = "Level.LSTR";
                    break;
                case 3:
                    retVal = "Level.L2";
                    break;
                case 4:
                    retVal = "Level.L3";
                    break;
                default:
                    retVal = "Level." + str;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Takes the string provided and returns the corresponding Level enum
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string format_level(Generated.acceptor.ST_LEVEL level)
        {
            string retVal = "";

            switch (level)
            {
                case Generated.acceptor.ST_LEVEL.StLevel_L0:
                    retVal = "Level.L0";
                    break;
                case Generated.acceptor.ST_LEVEL.StLevel_L1:
                    retVal = "Level.L1";
                    break;
                case Generated.acceptor.ST_LEVEL.StLevel_LSTM:
                    retVal = "Level.LSTR";
                    break;
                case Generated.acceptor.ST_LEVEL.StLevel_L2:
                    retVal = "Level.L2";
                    break;
                case Generated.acceptor.ST_LEVEL.StLevel_L3:
                    retVal = "Level.L3";
                    break;
                default:
                    retVal = "Level.Unknown";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Takes the string provided and returns the formatted string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string format_str(string str)
        {
            if (str == null)
            {
                str = "";
            }

            int i, j = 0;

            i = str.IndexOf("(");
            if (i >= 0)
            {
                j = str.IndexOf(")", i);
            }

            while (i >= 0 && j >= 0)
            {
                str = str.Substring(0, i) + str.Substring(j + 1);

                i = str.IndexOf("(");
                if (i >= 0)
                {
                    j = str.IndexOf(")", i);
                }
            }

            return str.Trim();
        }

        /// <summary>
        /// Takes the string provided and returns the corresponding decimal value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int format_decimal(string str)
        {
            int retVal = -1;

            str = format_str(str);
            if (str.EndsWith("d"))
            {
                str = str.Substring(0, str.Length - 1).Trim();
                try
                {
                    retVal = int.Parse(str);
                }
                catch (FormatException)
                {
                }
            }
            else if (str.EndsWith("b"))
            {
                str = str.Substring(0, str.Length - 1).Trim();
                char[] tmp = str.ToCharArray();
                retVal = 0;
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i] == '0')
                    {
                        retVal = retVal * 2;
                    }
                    else if (tmp[i] == '1')
                    {
                        retVal = retVal * 2 + 1;
                    }
                }
            }
            else
            {
                try
                {
                    retVal = int.Parse(str);
                }
                catch (FormatException)
                {
                }
            }

            return retVal;
        }

        /// <summary>
        /// Takes the string provided and returns the corresponding decimal value, as a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string format_decimal_as_str(string str)
        {
            return "" + format_decimal(str);
        }

        /// <summary>
        /// Adds a model element in this model element
        /// </summary>
        /// <param name="copy"></param>
        public override void AddModelElement(Utils.IModelElement element)
        {
            SubStep subStep = element as SubStep;
            if (subStep != null)
            {
                appendSubSteps(subStep);
            }
        }

    }
}
