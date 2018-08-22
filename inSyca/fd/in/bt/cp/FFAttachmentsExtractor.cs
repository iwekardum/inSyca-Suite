﻿///
///
///
///
///
///
///
///
///
///
///
///
///

using inSyca.foundation.integration.biztalk.components.diagnostics;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace inSyca.foundation.integration.biztalk.components
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_DisassemblingParser)]
    [Guid("16D92957-2034-4853-840B-BD424C596247")]
    public class FFAttachmentsExtractor : FFDasmComp, IBaseComponent, IComponentUI, IDisassemblerComponent, IPersistPropertyBag
    {
        static private readonly ResourceManager _resourceManager = new ResourceManager("inSyca.foundation.integration.biztalk.components.Properties.Resources", Assembly.GetExecutingAssembly());
       
        static string RegExReplacementLabel = "RegExReplacement";
        static string RemoveEmptyLinesLabel = "RemoveEmptyLines";

        #region Properties

        [Description("RegExReplacement")]
        [DisplayName("RegExReplacement")]
        [DefaultValue("\".*?\"")]
        public string RegExReplacement { get; set; }

        [Description("RemoveEmptyLines")]
        [DisplayName("RemoveEmptyLines")]
        [DefaultValue(true)]
        public bool RemoveEmptyLines { get; set; }
        #endregion

        #region IBaseComponent Members

        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public new string Description
        {
            get
            {
                return "FlatFile Email Attachments Extractor";
            }
        }

        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public new string Name
        {
            get
            {
                return "FFEmailAttachmentsExtractor";
            }
        }

        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public new string Version
        {
            get
            {
                return "1.0";
            }
        }
        #endregion

        #region IPersistPropertyBag Members

        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classID">
        /// Class ID of the component
        /// </param>
        public new void GetClassID(out Guid classID)
        {
            classID = new System.Guid("16D92957-2034-4853-840B-BD424C596247");

            Log.DebugFormat("GetClassID(out Guid {0})", classID);
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public new void InitNew()
        {
            Log.Debug("InitNew()");
        }

        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="propertyBag">Configuration property bag</param>
        /// <param name="errorLog">Error status</param>
		public new void Load(IPropertyBag propertyBag, int errorLog)
        {
            Log.DebugFormat("Load(IPropertyBag propertyBag {0} , int errorLog {1})", propertyBag, errorLog);

            base.Load(propertyBag, errorLog);

            using (DisposableObjectWrapper wrapper = new DisposableObjectWrapper(propertyBag))
            {
                object val = null;

                val = PropertyHelper.ReadPropertyBag(propertyBag, RegExReplacementLabel);
               
                if (val != null)
                    RegExReplacement = (string)val;
                
				val = PropertyHelper.ReadPropertyBag(propertyBag, RemoveEmptyLinesLabel);
               
                if (val != null)
                    RemoveEmptyLines = (bool)val;
            }

            Log.DebugFormat("Load RegExReplacement {0}", RegExReplacement);
            Log.DebugFormat("Load RemoveEmptyLines {0}", RemoveEmptyLines);
        }


        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="propertyBag">Configuration property bag</param>
        /// <param name="clearDirty">not used</param>
        /// <param name="saveAllProperties">not used</param>
        public new void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            Log.DebugFormat("Load(IPropertyBag propertyBag {0}, bool clearDirty {1}, bool saveAllProperties {2})", propertyBag, clearDirty, saveAllProperties);

            base.Save(propertyBag, clearDirty, saveAllProperties);

            using (DisposableObjectWrapper wrapper = new DisposableObjectWrapper(propertyBag))
            {
                object val = null;

                val = RegExReplacement;
                propertyBag.Write(RegExReplacementLabel, ref val);

                val = RemoveEmptyLines;
                propertyBag.Write(RemoveEmptyLinesLabel, ref val);
            }

            Log.DebugFormat("Save RegExReplacement {0}", RegExReplacement);
            Log.DebugFormat("Save RemoveEmptyLines {0}", RemoveEmptyLines);
        }

        #endregion

        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        public new IntPtr Icon
        {
            get
            {
                return Properties.Resources.cog.Handle;
            }
        }

        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add("This is a compiler error");
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion

        /// <summary>
        /// this variable will contain any message generated by the Disassemble method
        /// </summary>
        private System.Collections.Queue _msgs = new System.Collections.Queue();

        #region IDisassemblerComponent members

        /// <summary>
        /// Returns messages resulting from the disassemble method execution
        /// </summary>
        /// <param name="pipelineContext">the pipeline context</param>
        /// <returns></returns>
        public new IBaseMessage GetNext(IPipelineContext pipelineContext)
        {
            Log.DebugFormat("GetNext(IPipelineContext pc {0})", pipelineContext);
            // get the next message from the Queue and return it
            IBaseMessage msg = null;
            if (_msgs.Count > 0)
            {
                Log.DebugFormat("_msgs.Count {0}", _msgs.Count);

                msg = ((IBaseMessage)(_msgs.Dequeue()));

                return msg;
            }
            else
            {
                Log.DebugFormat("Queue empty");
                return msg;
            }
        }

        /// <summary>
        /// called by the messaging engine when a new message arrives
        /// </summary>
        /// <param name="pipelineContext">the pipeline context</param>
        /// <param name="inMsg">the actual message</param>
        public new void Disassemble(Microsoft.BizTalk.Component.Interop.IPipelineContext pipelineContext, Microsoft.BizTalk.Message.Interop.IBaseMessage inMsg)
        {
            Log.DebugFormat("Execute(IPipelineContext pContext {0}, IBaseMessage pInMsg {1})", pipelineContext, inMsg);

            var partName = string.Empty;
            // we start from index 1 because index zero contains the body of the message
            // which we are not interested
            for (int i = 1; i < inMsg.PartCount; i++)
            {
                Log.DebugFormat("PartCounter {0}", i);

                IBaseMessagePart currentPart = inMsg.GetPartByIndex(i, out partName);

                Stream currentPartStream = currentPart.GetOriginalDataStream();

                IBaseMessage outMsg;
                outMsg = pipelineContext.GetMessageFactory().CreateMessage();

                for (int j = 0; j < inMsg.Context.CountProperties; j++)
                {

                    string currentName;
                    string currentNamespace;
                    object obj = inMsg.Context.ReadAt(j, out currentName, out currentNamespace);
                    outMsg.Context.Write(currentName, currentNamespace, obj);

                    if (inMsg.Context.IsPromoted(currentName, currentNamespace))
                    {
                        outMsg.Context.Promote(currentName, currentNamespace, obj);
                    }
                }

                if (IsValidImage(currentPartStream))
                {
                    continue;
                }

                Stream ms;
                StreamReader sr = new StreamReader(currentPartStream);
                string messageString = sr.ReadToEnd();
                Log.DebugFormat("messageString before processing{0}", messageString);

                if (RemoveEmptyLines)
                {
                    Log.DebugFormat("RemoveEmptyLinesLabel {0}", RemoveEmptyLines);

                    messageString = messageString.TrimEnd('\r', '\n');
                    messageString = messageString + Environment.NewLine;
                }

                if (!string.IsNullOrEmpty(RegExReplacement))
                {
                    Log.DebugFormat("RegExReplacementLabel {0}", RegExReplacement);
                    //                    Regex rgx = new Regex("\".*?\"");

                    try
                    {
                        Regex rgx = new Regex(RegExReplacement);
                        messageString = rgx.Replace(messageString, "");
                    }
                    catch (Exception ex)
                    {
                        Log.Error("RegExReplacement Error {0}", ex);
                    }
                }

                Log.DebugFormat("messageString after processing{0}", messageString);
                byte[] byteArray = Encoding.UTF8.GetBytes(messageString);
                ms = new MemoryStream(byteArray);

                ms.Seek(0, SeekOrigin.Begin);
                outMsg.AddPart("Body", pipelineContext.GetMessageFactory().CreateMessagePart(), true);
                outMsg.BodyPart.Data = ms;


                //Promote attachment file name
                outMsg.Context.Write("ReceivedFileName", "http://schemas.microsoft.com/BizTalk/2003/file-properties", currentPart.PartProperties.Read("FileName", "http://schemas.microsoft.com/BizTalk/2003/mime-properties"));

                base.Disassemble(pipelineContext, outMsg);

                _msgs.Enqueue(base.GetNext(pipelineContext));

                Log.DebugFormat("Flatfile Dissasembler(IPipelineContext pContext {0}, IBaseMessage outMsg {1})", pipelineContext, outMsg);

                //_msgs.Enqueue(outMsg);
            }
        }
        private bool IsValidImage(Stream ms)
        {
            try
            {
                Image.FromStream(ms);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}