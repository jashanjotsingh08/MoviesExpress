using Lab3_Arshdeep_Jashanjot_.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_Arshdeep_Jashanjot_.Service
{
    interface ITableDataService
    {
        void Store<T>(T item) where T : new();
        void BatchStore<T>(IEnumerable<T> items) where T : class;
        IEnumerable<T> GetAll<T>(string name) where T : class;
        T GetItem<T>(string key) where T : class;
        void UpdateItem<T>(T item) where T : class;
    }
}
