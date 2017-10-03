using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BWS.Utils.AspNetCore.Format {
    public static class RegexHelper {

        public static Match NamesMatch(string expre,string content) => new Regex(expre).Match(content);

        public static MatchCollection AllNamesMatch(string expre, string content) => new Regex(expre).Matches(content);

        public static bool HasValue(this Match coll,string name) => coll.Groups[name].Value != "";

        public static string TryGet(this Match coll, string name) => coll.Groups[name].Value;

    }
}
