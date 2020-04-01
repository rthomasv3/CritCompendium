using CritCompendiumInfrastructure.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CritCompendiumInfrastructure
{
   public static class DependencyResolver
   {
      #region Fields

      private static Dictionary<Type, object> _singleInstanceObjects;
      private static Dictionary<Type, Type> _interfaceMap;

      #endregion

      #region Constructor

      /// <summary>
      /// Initializes <see cref="DependencyResolver"/>
      /// </summary>
      static DependencyResolver()
      {
         _singleInstanceObjects = new Dictionary<Type, object>();
         _interfaceMap = new Dictionary<Type, Type>();
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Registers an instance of the object
      /// </summary>
      public static void RegisterInstance(object obj)
      {
         if (obj != null)
         {
            _singleInstanceObjects[obj.GetType()] = obj;
         }
      }

      /// <summary>
      /// Registers an interface to concrete type mapping
      /// </summary>
      public static void RegisterInterface(Type interfaceType, Type concreteType)
      {
         _interfaceMap[interfaceType] = concreteType;
      }

      /// <summary>
      /// Resolves instance
      /// </summary>
      public static T Resolve<T>()
      {
         return (T)Resolve(typeof(T));
      }

      #endregion

      #region Private Methods

      private static object Resolve(Type type)
      {
         object obj = null;

         try
         {
            if (type.IsInterface)
            {
               if (_interfaceMap.ContainsKey(type))
               {
                  obj = Resolve(_interfaceMap[type]);
               }
            }
            else if (_singleInstanceObjects.ContainsKey(type))
            {
               obj = _singleInstanceObjects[type];
            }
            else if (type.IsPrimitive)
            {
               obj = Activator.CreateInstance(type);
            }
            else
            {
               ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

               if (constructor != null)
               {
                  obj = Activator.CreateInstance(type);
               }
               else
               {
                  constructor = type.GetConstructors().FirstOrDefault();

                  if (constructor != null)
                  {
                     List<object> parameters = new List<object>();

                     foreach (ParameterInfo parameter in constructor.GetParameters())
                     {
                        parameters.Add(Resolve(parameter.ParameterType));
                     }

                     obj = Activator.CreateInstance(type, parameters.ToArray());
                  }
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }

         return obj;
      }

      #endregion
   }
}
