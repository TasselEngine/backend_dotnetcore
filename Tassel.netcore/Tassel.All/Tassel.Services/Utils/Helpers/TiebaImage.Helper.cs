using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tassel.Services.Utils.Helpers {

    public static class TiebaImageHelper {

        public const string TiebaImgRoot = "system/images/tieba";

        private static Dictionary<string, string> middle_tieba_images = CreateTiebaGroup();
        private static List<KeyValuePair<string, string>> middle_tieba_images_list;

        private static Dictionary<string, string> TiebaModdleGroupMap { get => middle_tieba_images; }
        public static IList<KeyValuePair<string, string>> TiebaModdleGroup {
            get => middle_tieba_images_list ?? (middle_tieba_images_list = middle_tieba_images.ToList());
        }

        private static Dictionary<string, string> CreateTiebaGroup(string flag = "middle", string root = "/resources") {
            var prefix = $"{root}/{TiebaImgRoot}/{flag}";
            return new Dictionary<string, string> {
                ["#(呵呵)"]=$"{prefix}/1.png",
                ["#(高兴)"] = $"{prefix}/2.png",
                ["#(吐舌)"] = $"{prefix}/3.png",
                ["#(惊讶)"] = $"{prefix}/4.png",
                ["#(酷)"] = $"{prefix}/5.png",
                ["#(龇牙)"] = $"{prefix}/6.png",
                ["#(微笑)"] = $"{prefix}/7.png",
                ["#(流汗)"] = $"{prefix}/8.png",
                ["#(大哭)"] = $"{prefix}/9.png",
                ["#(尴尬)"] = $"{prefix}/10.png",
                ["#(鄙视)"] = $"{prefix}/11.png",
                ["#(无语)"] = $"{prefix}/12.png",
                ["#(棒)"] = $"{prefix}/13.png",
                ["#(财迷)"] = $"{prefix}/14.png",
                ["#(疑惑)"] = $"{prefix}/15.png",
                ["#(阴险)"] = $"{prefix}/16.png",
                ["#(呕吐)"] = $"{prefix}/17.png",
                ["#(咦)"] = $"{prefix}/18.png",
                ["#(委屈)"] = $"{prefix}/19.png",
                ["#(色)"] = $"{prefix}/20.png",
                ["#(泥马)"] = $"{prefix}/21.png",
                ["#(大笑)"] = $"{prefix}/22.png",
                ["#(冷)"] = $"{prefix}/23.png",
                ["#(开心)"] = $"{prefix}/24.png",
                ["#(滑稽)"] = $"{prefix}/25.png",
                ["#(拜托)"] = $"{prefix}/26.png",
                ["#(大汗)"] = $"{prefix}/27.png",
                ["#(可怜)"] = $"{prefix}/28.png",
                ["#(打瞌睡)"] = $"{prefix}/29.png",
                ["#(哭)"] = $"{prefix}/30.png",
                ["#(愤怒)"] = $"{prefix}/31.png",
                ["#(奥)"] = $"{prefix}/32.png",
                ["#(喷)"] = $"{prefix}/33.png",
                ["#(喜欢)"] = $"{prefix}/46.png",
                ["#(yeah)"] = $"{prefix}/47.png",
                ["#(赞)"] = $"{prefix}/48.png",
                ["#(垃圾)"] = $"{prefix}/49.png",
                ["#(ok)"] = $"{prefix}/50.png",
            };
        }

    }
}
