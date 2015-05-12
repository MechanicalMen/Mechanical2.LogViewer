using System;
using System.Threading;
using Mechanical.Collections;
using Mechanical.Conditions;
using Mechanical.Core;
using Mechanical.DataStores;
using Mechanical.MVVM;

namespace Mechanical.LogViewer.ViewModels
{
    public class ExceptionViewModel : ViewModelBase
    {
        public ExceptionViewModel( params KeyValueViewModel[] nodes )
            : base()
        {
            Ensure.That(nodes).NotNullEmptyOrSparse();

            this.Nodes = nodes;

            this.PropertyChange.Handle(this, Reveal.Name(() => this.SelectedNode), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
            this.PropertyChange.Handle(this, Reveal.Name(() => this.SafeStringPrint), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
            this.PropertyChange.Handle(this, Reveal.Name(() => this.HtmlDecode), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
            this.PropertyChange.Handle(this, Reveal.Name(() => this.UrlDecode), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
            this.PropertyChange.Handle(this, Reveal.Name(() => this.JsonEscape), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
            this.PropertyChange.Handle(this, Reveal.Name(() => this.JsonUnescape), () => this.RaisePropertyChanged(Reveal.Name(() => this.Value)));
        }

        public static ExceptionViewModel From( ExceptionInfo info )
        {
            return new ExceptionViewModel(KeyValueViewModel.From(info));
        }

        public KeyValueViewModel[] Nodes
        {
            get;
            private set;
        }

        private KeyValueViewModel selectedNode = null;
        public KeyValueViewModel SelectedNode
        {
            get
            {
                return this.selectedNode;
            }
            set
            {
                if( !object.ReferenceEquals(this.selectedNode, value) )
                {
                    this.selectedNode = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #region Derived Properties

        private bool safeStringPrint = false;
        public bool SafeStringPrint
        {
            get
            {
                return this.safeStringPrint;
            }
            set
            {
                if( this.safeStringPrint != value )
                {
                    this.safeStringPrint = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool htmlDecode = false;
        public bool HtmlDecode
        {
            get
            {
                return this.htmlDecode;
            }
            set
            {
                if( this.htmlDecode != value )
                {
                    this.DisableStringProcessing();
                    this.htmlDecode = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool urlDecode = false;
        public bool UrlDecode
        {
            get
            {
                return this.urlDecode;
            }
            set
            {
                if( this.urlDecode != value )
                {
                    this.DisableStringProcessing();
                    this.urlDecode = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool jsonEscape = false;
        public bool JsonEscape
        {
            get
            {
                return this.jsonEscape;
            }
            set
            {
                if( this.jsonEscape != value )
                {
                    this.DisableStringProcessing();
                    this.jsonEscape = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool jsonUnescape = false;
        public bool JsonUnescape
        {
            get
            {
                return this.jsonUnescape;
            }
            set
            {
                if( this.jsonUnescape != value )
                {
                    this.DisableStringProcessing();
                    this.jsonUnescape = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Value
        {
            get
            {
                if( this.SelectedNode.NullReference() )
                    return null;

                var result = this.SelectedNode.RawValue;
                try
                {
                    if( this.HtmlDecode )
                        result = System.Net.WebUtility.HtmlDecode(result);
                    else if( this.UrlDecode )
                        result = System.Net.WebUtility.UrlDecode(result);
                    else if( this.JsonEscape )
                    {
                        var textWriter = new Mechanical.IO.StringWriter();
                        using( var writer = new Mechanical.FileFormats.JsonWriter(textWriter, indent: false, produceAscii: true) )
                        {
                            writer.WriteArrayStart();
                            writer.WriteValue(result);
                            writer.WriteArrayEnd();
                        }
                        result = textWriter.ToString();
                        result = result.Substring(startIndex: 2, length: result.Length - 4);
                        textWriter.Close();
                    }
                    else if( this.JsonUnescape )
                    {
                        var textReader = new Mechanical.IO.StringReader("[\"" + result + "\"]");
                        using( var reader = new Mechanical.FileFormats.JsonReader(textReader) )
                        {
                            reader.Read();
                            reader.Read();
                            result = reader.RawValue;
                        }
                        textReader.Close();
                    }
                }
                catch
                {
                    result = null;
                }

                if( this.SafeStringPrint )
                    return SafeString.DebugPrint(result);
                else
                    return result;
            }
        }

        #endregion

        #region Private Methods

        private int isDisabling;
        private void DisableStringProcessing()
        {
            if( Interlocked.CompareExchange(ref this.isDisabling, 1, comparand: 0) == 0 )
            {
                this.HtmlDecode = false;
                this.UrlDecode = false;
                this.JsonEscape = false;
                this.JsonUnescape = false;
                Interlocked.Exchange(ref this.isDisabling, 0);
            }
        }

        #endregion
    }
}
