using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Web;

namespace EIDService.Common.Entities
{
    public interface IEntity
    {
        string Key { get; }
    }

    public abstract class Entity<TId> : IEntity
    {
        public TId Id { get; set; }

        [StringLength(50)]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }

        private string _key;

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public static class KeyGenerator
        {
            public static string Generate()
            {
                return Generate(Guid.NewGuid().ToString("D").Substring(24));
            }

            public static string Generate(string input)
            {

                Contract.Requires(!string.IsNullOrWhiteSpace(input));

                return HttpUtility.UrlEncode(input.Replace(" ", "_").Replace("-", "_").Replace("&", "and"));

            }

        }
    }
}
