using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace septa.Auth.Domain.Hellper
{
    public static class StringExtensions
    {
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.NotNull<string>(str, nameof(str));
            if (str.EndsWith(c.ToString(), comparisonType))
                return str;
            return str + c.ToString();
        }

        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.NotNull<string>(str, nameof(str));
            if (str.StartsWith(c.ToString(), comparisonType))
                return str;
            return c.ToString() + str;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string Left(this string str, int len)
        {
            Check.NotNull<string>(str, nameof(str));
            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            return str.Substring(0, len);
        }

        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        public static int NthIndexOf(this string str, char c, int n)
        {
            Check.NotNull<string>(str, nameof(str));
            int num = 0;
            for (int index = 0; index < str.Length; ++index)
            {
                if ((int)str[index] == (int)c && ++num == n)
                    return index;
            }
            return -1;
        }

        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }

        public static string RemovePostFix(
          this string str,
          StringComparison comparisonType,
          params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
                return (string)null;
            if (((ICollection<string>)postFixes).IsNullOrEmpty<string>())
                return str;
            foreach (string postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                    return str.Left(str.Length - postFix.Length);
            }
            return str;
        }

        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        public static string RemovePreFix(
          this string str,
          StringComparison comparisonType,
          params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
                return (string)null;
            if (((ICollection<string>)preFixes).IsNullOrEmpty<string>())
                return str;
            foreach (string preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                    return str.Right(str.Length - preFix.Length);
            }
            return str;
        }

        public static string ReplaceFirst(
          this string str,
          string search,
          string replace,
          StringComparison comparisonType = StringComparison.Ordinal)
        {
            Check.NotNull<string>(str, nameof(str));
            int length = str.IndexOf(search, comparisonType);
            if (length < 0)
                return str;
            return str.Substring(0, length) + replace + str.Substring(length + search.Length);
        }

        public static string Right(this string str, int len)
        {
            Check.NotNull<string>(str, nameof(str));
            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            return str.Substring(str.Length - len, len);
        }

        public static string[] Split(this string str, string separator)
        {
            return str.Split(new string[1] { separator }, StringSplitOptions.None);
        }

        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new string[1] { separator }, options);
        }

        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        public static string ToCamelCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            if (str.Length != 1)
                return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])).ToString() + str.Substring(1);
            if (!useCurrentCulture)
                return str.ToLowerInvariant();
            return str.ToLower();
        }

        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            if (!useCurrentCulture)
                return Regex.Replace(str, "[a-z][A-Z]", (MatchEvaluator)(m =>
                {
                    char lowerInvariant = m.Value[0];
                    string str1 = lowerInvariant.ToString();
                    lowerInvariant = char.ToLowerInvariant(m.Value[1]);
                    string str2 = lowerInvariant.ToString();
                    return str1 + " " + str2;
                }));
            return Regex.Replace(str, "[a-z][A-Z]", (MatchEvaluator)(m =>
            {
                char lower = m.Value[0];
                string str1 = lower.ToString();
                lower = char.ToLower(m.Value[1]);
                string str2 = lower.ToString();
                return str1 + " " + str2;
            }));
        }

        public static string ToKebabCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            str = str.ToCamelCase(false);
            if (!useCurrentCulture)
                return Regex.Replace(str, "[a-z][A-Z]", (MatchEvaluator)(m =>
                {
                    char lowerInvariant = m.Value[0];
                    string str1 = lowerInvariant.ToString();
                    lowerInvariant = char.ToLowerInvariant(m.Value[1]);
                    string str2 = lowerInvariant.ToString();
                    return str1 + "-" + str2;
                }));
            return Regex.Replace(str, "[a-z][A-Z]", (MatchEvaluator)(m =>
            {
                char lower = m.Value[0];
                string str1 = lower.ToString();
                lower = char.ToLower(m.Value[1]);
                string str2 = lower.ToString();
                return str1 + "-" + str2;
            }));
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            Check.NotNull<string>(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            Check.NotNull<string>(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte num in hash)
                    stringBuilder.Append(num.ToString("X2"));
                return stringBuilder.ToString();
            }
        }

        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            if (str.Length != 1)
                return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])).ToString() + str.Substring(1);
            if (!useCurrentCulture)
                return str.ToUpperInvariant();
            return str.ToUpper();
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
                return (string)null;
            if (str.Length <= maxLength)
                return str;
            return str.Left(maxLength);
        }

        public static string TruncateFromBeginning(this string str, int maxLength)
        {
            if (str == null)
                return (string)null;
            if (str.Length <= maxLength)
                return str;
            return str.Right(maxLength);
        }

        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return str.TruncateWithPostfix(maxLength, "...");
        }

        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
                return (string)null;
            if (str == string.Empty || maxLength == 0)
                return string.Empty;
            if (str.Length <= maxLength)
                return str;
            if (maxLength <= postfix.Length)
                return postfix.Left(maxLength);
            return str.Left(maxLength - postfix.Length) + postfix;
        }

        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            Check.NotNull<string>(str, nameof(str));
            Check.NotNull<Encoding>(encoding, nameof(encoding));
            return encoding.GetBytes(str);
        }
    }

}
