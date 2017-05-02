using System;
using System.Reflection;
using Ataoge.Extensions;

namespace Ataoge.Security
{
    /// <summary>
    /// Used to identify a user.
    /// </summary>
    public class UserIdentifier<TKey> : IUserIdentifier<TKey>
        where TKey :  IEquatable<TKey>

    {
         /// <summary>
        /// Tenant Id of the user.
        /// Can be null for host users in a multi tenant application.
        /// </summary>
        public int? TenantId { get; protected set; }

        /// <summary>
        /// Id of the user.
        /// </summary>
        public TKey UserId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifier"/> class.
        /// </summary>
        protected UserIdentifier()
        {

        }

         /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifier"/> class.
        /// </summary>
        /// <param name="tenantId">Tenant Id of the user.</param>
        /// <param name="userId">Id of the user.</param>
        public UserIdentifier(int? tenantId, TKey userId)
        {
            TenantId = tenantId;
            UserId = userId;
        }

        /// <summary>
        /// Parses given string and creates a new <see cref="UserIdentifier"/> object.
        /// </summary>
        /// <param name="userIdentifierString">
        /// Should be formatted one of the followings:
        /// 
        /// - "userId@tenantId". Ex: "42@3" (for tenant users).
        /// - "userId". Ex: 1 (for host users)
        /// </param>
        public static UserIdentifier<TKey> Parse(string userIdentifierString)
        {
            if (string.IsNullOrEmpty(userIdentifierString))
            {
                throw new ArgumentNullException(nameof(userIdentifierString), "userAtTenant can not be null or empty!");
            }

            var splitted = userIdentifierString.Split('@');
            if (splitted.Length == 1)
            {
                return new UserIdentifier<TKey>(null, ConvertIdFromString(splitted[0]));

            }

            if (splitted.Length == 2)
            {
                return new UserIdentifier<TKey>(splitted[1].To<int>(), ConvertIdFromString(splitted[0]));
            }

            throw new ArgumentException("userAtTenant is not properly formatted", nameof(userIdentifierString));
        }

        private static TKey ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(TKey); 
            }
            return (TKey)Convert.ChangeType(id, typeof(TKey));
            //return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        /// <summary>
        /// Creates a string represents this <see cref="UserIdentifier"/> instance.
        /// Formatted one of the followings:
        /// 
        /// - "userId@tenantId". Ex: "42@3" (for tenant users).
        /// - "userId". Ex: 1 (for host users)
        /// 
        /// Returning string can be used in <see cref="Parse"/> method to re-create identical <see cref="UserIdentifier"/> object.
        /// </summary>
        public string ToUserIdentifierString()
        {
            if (TenantId == null)
            {
                return UserId.ToString();
            }

            return UserId + "@" + TenantId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserIdentifier<TKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (UserIdentifier<TKey>)obj;

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return TenantId == other.TenantId && UserId.Equals(other.UserId);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return TenantId == null ? UserId.GetHashCode() : ToUserIdentifierString().GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(UserIdentifier<TKey> left, UserIdentifier<TKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(UserIdentifier<TKey> left, UserIdentifier<TKey> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return ToUserIdentifierString();
        }
    }
}