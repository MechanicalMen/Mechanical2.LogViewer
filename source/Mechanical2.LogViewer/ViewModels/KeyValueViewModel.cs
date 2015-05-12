using System;
using System.Collections.Generic;
using System.Threading;
using Mechanical.Collections;
using Mechanical.Conditions;
using Mechanical.Core;
using Mechanical.DataStores;
using Mechanical.MVVM;

namespace Mechanical.LogViewer.ViewModels
{
    public class KeyValueViewModel : PropertyChangedBase
    {
        #region Constructors

        public KeyValueViewModel( string key, string value )
        {
            this.Key = key;
            this.RawValue = value;
            this.Children = null;
        }

        public KeyValueViewModel( string key, KeyValueViewModel[] children )
        {
            this.Key = key;
            this.RawValue = null;
            this.Children = children.NullOrEmpty() ? null : children;
        }

        #endregion

        #region Public Properties

        public string Key
        {
            get;
            private set;
        }

        public string RawValue
        {
            get;
            private set;
        }

        public KeyValueViewModel[] Children
        {
            get;
            private set;
        }

        private static readonly char[] NewLineChars = new char[] { '\r', '\n' };
        public string FirstLineOfValue
        {
            get
            {
                if( this.RawValue.NullReference() )
                    return null;

                int at = this.RawValue.IndexOfAny(NewLineChars);
                if( at == -1 )
                    return this.RawValue;
                else
                    return this.RawValue.Substring(startIndex: 0, length: at) + "...";
            }
        }

        #endregion

        #region Static Methods

        private static KeyValueViewModel[] FromStore( ExceptionInfo info )
        {
            var array = new KeyValueViewModel[info.Store.Count];
            int i = 0;
            foreach( var pair in info.Store )
            {
                array[i] = new KeyValueViewModel(pair.Key, pair.Value);
                ++i;
            }
            return array;
        }

        private static KeyValueViewModel[] FromInnerExceptions( ExceptionInfo info )
        {
            var array = new KeyValueViewModel[info.InnerExceptions.Count];
            for( int i = 0; i < info.InnerExceptions.Count; ++i )
                array[i] = new KeyValueViewModel(SafeString.DebugFormat("[{0}]", i), From(info.InnerExceptions[i]));
            return array;
        }

        public static KeyValueViewModel[] From( ExceptionInfo info )
        {
            Ensure.That(info).NotNull();

            var list = new List<KeyValueViewModel>(capacity: 5);
            list.Add(new KeyValueViewModel("Type", info.Type));
            list.Add(new KeyValueViewModel("Message", info.Message));
            if( info.Store.Count != 0 )
                list.Add(new KeyValueViewModel("Store", FromStore(info)));
            if( !info.StackTrace.NullOrEmpty() )
                list.Add(new KeyValueViewModel("StackTrace", info.StackTrace));

            if( info.InnerExceptions.Count == 1 )
                list.Add(new KeyValueViewModel("InnerException", From(info.InnerException)));
            else if( info.InnerExceptions.Count > 1 )
                list.Add(new KeyValueViewModel("InnerExceptions", FromInnerExceptions(info)));

            return list.ToArray();
        }

        #endregion
    }
}
