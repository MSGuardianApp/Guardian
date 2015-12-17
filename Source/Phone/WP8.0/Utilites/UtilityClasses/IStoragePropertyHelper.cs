﻿using System.IO.IsolatedStorage;

namespace SOS.Phone
{
    internal static class IsolatedStoragePropertyHelper    
    {        
        /// <summary>        
        /// We must use this object to lock saving settings        
        /// </summary>        
        public static readonly object ThreadLocker = new object();         
        public static readonly IsolatedStorageSettings Store = IsolatedStorageSettings.ApplicationSettings;    
    }

    /// <summary>    
    /// This is wrapper class for storing one setting    
    /// Object of this type must be single    
    /// </summary>    
    /// <typeparam name="T">Any serializable type</typeparam>    
    public class IsolatedStorageProperty<T>    
    {        
        private readonly object _defaultValue;        
        private readonly string _name;        
        private readonly object _syncObject = new object();         

        public IsolatedStorageProperty(string name, T defaultValue = default(T))        
        {            
            _name = name;            
            _defaultValue = defaultValue;        
        }
         
        /// <summary>        
        /// Determines if setting exists in the storage        
        /// </summary>        
        public bool Exists        
        {            
            get 
            { 
                return IsolatedStoragePropertyHelper.Store.Contains(_name); 
            }        
        }         
        
        /// <summary>        
        /// Use this property to access the actual setting value        
        /// </summary>        
        public T Value        
        {            
            get            
            {                
                //If property does not exist - initializing it using default value                
                if (!Exists)                
                {                    
                    //Initializing only once                    
                    lock (_syncObject)                    
                    {                        
                        if (!Exists) SetDefault();                    
                    }                
                }                 
                return (T) IsolatedStoragePropertyHelper.Store[_name];            
            }            
            set            
            {                
                IsolatedStoragePropertyHelper.Store[_name] = value;                
                Save();            
            }        
        }
         
        private static void Save()        
        {            
            lock (IsolatedStoragePropertyHelper.ThreadLocker)            
            {                
                IsolatedStoragePropertyHelper.Store.Save();            
            }        
        }         
        public void SetDefault()        
        {            
            Value = (T) _defaultValue;        
        }

        public void Remove(string key)
        {
            lock (IsolatedStoragePropertyHelper.ThreadLocker)
            {
                IsolatedStoragePropertyHelper.Store.Remove(key);
            }
        }

    }
}
