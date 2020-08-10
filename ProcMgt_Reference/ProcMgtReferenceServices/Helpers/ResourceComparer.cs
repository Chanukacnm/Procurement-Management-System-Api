using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using ProcMgt_Reference_Services.Helpers;

namespace ProcMgt_Reference_Services.Helpers
{
    public class ResourceComparer<T> where T : class
    {
        private T ObjSource;
        private T ObjTarget;
        private bool updated = false;
        private bool deleted = false;

        public ResourceComparer(T objSource, T objTarget)
        {
            ObjSource = objSource;
            ObjTarget = objTarget;
        }

        public ResourceComparerResult<T> GetUpdatedObject()
        {

            PropertyInfo[] propInfo = ObjTarget.GetType().GetProperties();
            foreach (PropertyInfo pi in propInfo)
            {
                var attribute = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;

                if (attribute == null)
                {
                     if (!(pi.GetValue(ObjSource) == null && pi.GetValue(ObjTarget) == null))
                     { 
                        if(pi.GetValue(ObjSource) == null)
                        {
                            pi.SetValue(ObjTarget, null);
                            updated = true;
                        }else if (!(pi.GetValue(ObjSource).Equals(pi.GetValue(ObjTarget))))
                        {
                            pi.SetValue(ObjTarget, pi.GetValue(ObjSource));
                            updated = true;
                        }
                     }
                }
                

            }

            //if (attribute == null)
            //{
            //    if (!(pi.GetValue(ObjSource).Equals(pi.GetValue(ObjTarget))))
            //    {
            //        pi.SetValue(ObjTarget, pi.GetValue(ObjSource));
            //        updated = true;
            //    }
            //}

            //if (attribute == pi.GetValue(ObjTarget))
            //{
            //    pi.SetValue(ObjTarget, pi.GetValue(ObjSource));
            //}
            return new ResourceComparerResult<T>(ObjTarget, updated);
        }


        public ResourceComparerResult<T> GetDeletedObject()
        {

            PropertyInfo[] propInfo = ObjTarget.GetType().GetProperties();
            foreach (PropertyInfo pi in propInfo)
            {
                var attribute = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;

                if (attribute == null)
                {
                    if (!(pi.GetValue(ObjSource).Equals(pi.GetValue(ObjTarget))))
                    {
                        pi.SetValue(ObjTarget, pi.GetValue(ObjSource));
                        updated = true;
                    }
                }
            }

            return new ResourceComparerResult<T>(ObjTarget, deleted);
        }
    }
}
