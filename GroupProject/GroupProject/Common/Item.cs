using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Common
{
    internal class Item
    {
        /// <summary>
        /// item id
        /// </summary>
        private string id;
        /// <summary>
        /// item description
        /// </summary>
        private string desc;
        /// <summary>
        /// item cost
        /// </summary>
        private string cost;

        /// <summary>
        /// item constructor
        /// </summary>
        /// <param name="id">item id</param>
        /// <param name="desc">item description</param>
        /// <param name="cost">item cost</param>
        /// <exception cref="Exception"></exception>
        public Item(string id, string desc, string cost)
        {
            try
            {
                this.id = id;
                this.desc = desc;
                this.cost = cost;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// getter item id
        /// </summary>
        public string ItemeId { get { return id; } }
        /// <summary>
        /// getter item description
        /// </summary>
        public string ItemDesc { get { return desc; } }
        /// <summary>
        /// getter item cost
        /// </summary>
        public string ItemeCost { get { return cost; } }

        /// <summary>
        /// to string override uses item description for string
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">exception</exception>
        public override string ToString()
        {
            try
            {
                return $"{ItemDesc}";
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
