using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tassel.Services.Utils.Helpers {

    public static class GlobalStickersHelper {

        public const string TiebaImgRoot = "system/images/tieba";
        public const string SinaRoot = "system/images/sina";

        private static List<KeyValuePair<string, string>> middle_tieba_images_list = CreateTiebaGroup().ToList();
        public static IList<KeyValuePair<string, string>> TiebaModdleGroup { get => middle_tieba_images_list; }

        private static List<KeyValuePair<string, string>> king_master_images_list = CreateSinaRolesGroup().ToList();
        public static IList<KeyValuePair<string, string>> SinaRolesGroup { get => king_master_images_list; }

        private static List<KeyValuePair<string, string>> sina_others_list = CreateSinaOthersGroup().ToList();
        public static IList<KeyValuePair<string, string>> SinaOthersGroup { get => sina_others_list; }

        private static List<KeyValuePair<string, string>> sina_pop_images_list = CreateSinaPopGroup().ToList();
        public static IList<KeyValuePair<string, string>> SinaPopGroup { get => sina_pop_images_list; }

        private static Dictionary<string, string> CreateTiebaGroup(string root = "/resources") {
            var prefix = $"{root}/{TiebaImgRoot}/middle";
            var huaji2 = $"{root}/{TiebaImgRoot}/others/huaji-extra";
            return new Dictionary<string, string> {
                ["#(呵呵)"] = $"{prefix}/1.png",
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
                ["#(葫芦滑稽1)"] = $"{huaji2}/1.png",
                ["#(葫芦滑稽2)"] = $"{huaji2}/2.png",
                ["#(葫芦滑稽3)"] = $"{huaji2}/3.png",
                ["#(葫芦滑稽4)"] = $"{huaji2}/4.png",
                ["#(葫芦滑稽5)"] = $"{huaji2}/5.png",
                ["#(葫芦滑稽6)"] = $"{huaji2}/6.png",
                ["#(葫芦滑稽7)"] = $"{huaji2}/7.png",
                ["#(白帽滑稽)"] = $"{huaji2}/11.png",
                ["#(滑稽双马尾)"] = $"{huaji2}/12.png",
                ["#(滑稽上火)"] = $"{huaji2}/14.png",
                ["#(滑稽牛魔王)"] = $"{huaji2}/16.png",
                ["#(滑稽猪八戒)"] = $"{huaji2}/17.png",
                ["#(滑稽博士)"] = $"{huaji2}/18.png",
                ["#(滑稽黄瓜)"] = $"{huaji2}/19.png",
            };
        }

        private static Dictionary<string, string> CreateSinaPopGroup(string root = "/resources") {
            var pop = $"{root}/{SinaRoot}/pop";
            return new Dictionary<string, string> {
                ["#(sina并不简单)"] = $"{pop}/mogician.png",
                ["#(sina笑哭X)"] = $"{pop}/xiaocry.png",
                ["#(sina掩面)"] = $"{pop}/xiaocry2.png",
                ["#(sina难过)"] = $"{pop}/sad.png",
                ["#(sina闭嘴吧你)"] = $"{pop}/shutup.png",
                ["#(sina啊啊啊啊)"] = $"{pop}/aaaaa.png",
                ["#(sina怒)"] = $"{pop}/angry.png",
                ["#(sina生气)"] = $"{pop}/angry2.png",
                ["#(sina白眼)"] = $"{pop}/baiyan.png",
                ["#(sina再见)"] = $"{pop}/bye.png",
                ["#(sina好想舔哇咔咔)"] = $"{pop}/suck.png",
                ["#(sina惨)"] = $"{pop}/can.png",
                ["#(sina吃瓜)"] = $"{pop}/chigua.png",
                ["#(sinaCNM)"] = $"{pop}/cnm.png",
                ["#(sina酷)"] = $"{pop}/cool.png",
                ["#(sina哭惹)"] = $"{pop}/cry.png",
                ["#(sina大哭)"] = $"{pop}/cry2.png",
                ["#(sina死了)"] = $"{pop}/dead.png",
                ["#(sina好吃)"] = $"{pop}/dilic.png",
                ["#(sina不懂)"] = $"{pop}/dontknow.png",
                ["#(sina鄙视)"] = $"{pop}/fk.png",
                ["#(sina尴尬)"] = $"{pop}/ganga.png",
                ["#(sina哈哈)"] = $"{pop}/haha.png",
                ["#(sina汉)"] = $"{pop}/han.png",
                ["#(sina开心)"] = $"{pop}/happy.png",
                ["#(sina开心鼓掌)"] = $"{pop}/happy2.png",
                ["#(sina嘿嘿)"] = $"{pop}/heihei.png",
                ["#(sina左哼哼)"] = $"{pop}/heng2.png",
                ["#(sina右哼哼)"] = $"{pop}/heng.png",
                ["#(sina滑稽)"] = $"{pop}/huaji.png",
                ["#(sina贱笑)"] = $"{pop}/jianxiao.png",
                ["#(sina可怜)"] = $"{pop}/kelian.png",
                ["#(sina敲)"] = $"{pop}/knock.png",
                ["#(sina抠鼻)"] = $"{pop}/koubi.png",
                ["#(sina困)"] = $"{pop}/kun.png",
                ["#(sina很困)"] = $"{pop}/kun2.png",
                ["#(sina困死了)"] = $"{pop}/kun3.png",
                ["#(sina卡哇伊)"] = $"{pop}/kwy.png",
                ["#(sina喜欢)"] = $"{pop}/like.png",
                ["#(sina么么哒)"] = $"{pop}/meme.png",
                ["#(sina喵喵喵)"] = $"{pop}/miaomiaomiao.png",
                ["#(sina盲尼)"] = $"{pop}/money.png",
                ["#(sina纳尼)"] = $"{pop}/nani.png",
                ["#(sina扇脸)"] = $"{pop}/puxxx.png",
                ["#(sina害羞)"] = $"{pop}/shy.png",
                ["#(sina生病)"] = $"{pop}/sick.png",
                ["#(sina虚弱)"] = $"{pop}/weak.png",
                ["#(sina呵呵)"] = $"{pop}/smile.png",
                ["#(sina开心笑)"] = $"{pop}/smile2.png",
                ["#(sina好恶心啊)"] = $"{pop}/uhhhh.png",
                ["#(sina不是故意的)"] = $"{pop}/unhappy.png",
                ["#(sina好多钱)"] = $"{pop}/wamoney.png",
                ["#(sina互粉)"] = $"{pop}/watch.png",
                ["#(sina小声)"] = $"{pop}/wsp.png",
                ["#(sina吃一大惊)"] = $"{pop}/wtf.png",
                ["#(sina捂脸)"] = $"{pop}/wulian.png",
                ["#(sina龇牙嘻嘻)"] = $"{pop}/xixi.png",
                ["#(sina调皮)"] = $"{pop}/yeah.png",
                ["#(sina淫荡笑)"] = $"{pop}/yindang.png",
                ["#(sina晕倒)"] = $"{pop}/yun.png",
            };
        }

        private static Dictionary<string, string> CreateSinaRolesGroup(string root = "/resources") {
            var role = $"{root}/{SinaRoot}/role";
            var emoji = $"{root}/{SinaRoot}/emoji";
            var king_master = $"{root}/{TiebaImgRoot}/others/king-master";
            return new Dictionary<string, string> {
                ["#(哆啦A梦笑)"] = $"{role}/ameng-haha.png",
                ["#(哆啦A梦花心)"] = $"{role}/ameng-huaxin.png",
                ["#(哆啦A梦调皮)"] = $"{role}/ameng-lueluelue.png",
                ["#(哆啦A梦美味)"] = $"{role}/ameng-nicefood.png",
                ["#(哆啦A梦斜眼)"] = $"{role}/ameng-smg.png",
                ["#(哆啦A梦吃惊)"] = $"{role}/ameng-waoh.png",
                ["#(浪小花开心)"] = $"{role}/sina-haha.png",
                ["#(浪小花喜欢)"] = $"{role}/sina-like.png",
                ["#(浪小花爱心)"] = $"{role}/sina-se.png",
                ["#(浪小花偷笑)"] = $"{role}/sina-xycz.png",
                ["#(浪小花财迷)"] = $"{role}/sina-youqian.png",
                ["#(浪小花请求)"] = $"{role}/sina-youqian2.png",
                ["#(小黄人不屑)"] = $"{role}/xhr-buxie.png",
                ["#(小黄人很不屑)"] = $"{role}/xhr-buxie2.png",
                ["#(小黄人内涵)"] = $"{role}/xhr-ding.png",
                ["#(小黄人摆手)"] = $"{role}/xhr-hei.png",
                ["#(小黄人兴奋)"] = $"{role}/xhr-jidong.png",
                ["#(小黄人得意)"] = $"{role}/xhr-tired.png",
                ["#(小黄人疑惑)"] = $"{role}/xhr-yihuo.png",
                ["#(小黄人奸笑)"] = $"{role}/xhr-yinxiao.png",
                ["#(小黄人莫名其妙)"] = $"{role}/xhr-zoubian.png",
                ["#(emoji合掌)"] = $"{emoji}/baoyou.png",
                ["#(emoji猥琐)"] = $"{emoji}/comeon2.png",
                ["#(emoji举手)"] = $"{emoji}/ghandsup.png",
                ["#(emoji小鬼)"] = $"{emoji}/ghost.png",
                ["#(emoji拒绝)"] = $"{emoji}/gno.png",
                ["#(emoji黑框眼镜)"] = $"{emoji}/hkyj.png",
                ["#(emoji坏蛋)"] = $"{emoji}/huairen.png",
                ["#(emoji该吃药了)"] = $"{emoji}/medicine.png",
                ["#(emoji我不听)"] = $"{emoji}/mknolisten.png",
                ["#(emoji说错话)"] = $"{emoji}/mkshutup.png",
                ["#(emoji我看不到)"] = $"{emoji}/mkshy.png",
                ["#(emoji哦呵呵)"] = $"{emoji}/ofuck.png",
                ["#(emoji啪啪啪)"] = $"{emoji}/papa.png",
                ["#(emoji害怕)"] = $"{emoji}/scared.png",
                ["#(emoji吃屎吧你)"] = $"{emoji}/shit.png",
                ["#(吃屎啊01)"] = $"{king_master}/1.png",
                ["#(恐怖01)"] = $"{king_master}/2.png",
                ["#(大佬别打01)"] = $"{king_master}/3.png",
                ["#(咦咦咦01)"] = $"{king_master}/4.png",
                ["#(咩咩咩01)"] = $"{king_master}/5.png",
                ["#(苦笑01)"] = $"{king_master}/6.png",
                ["#(自信01)"] = $"{king_master}/7.png",
                ["#(自信02)"] = $"{king_master}/8.png",
                ["#(吃屎啊02)"] = $"{king_master}/9.png",
                ["#(苦笑02)"] = $"{king_master}/10.png",
            };
        }

        private static Dictionary<string, string> CreateSinaOthersGroup(string root = "/resources") {
            var others = $"{root}/{SinaRoot}/others";
            return new Dictionary<string, string> {
                ["#(sinarDOGE)"] = $"{others}/doge.png",
                ["#(sinarCAT)"] = $"{others}/cat.png",
                ["#(sinarHSQ)"] = $"{others}/hsq.png",
                ["#(sinar熊猫)"] = $"{others}/panda.png",
                ["#(sinar兔纸)"] = $"{others}/rabbit.png",
                ["#(sinar猪头)"] = $"{others}/pig.png",
                ["#(sinar羊驼)"] = $"{others}/yangtuo.png",
                ["#(sinar摊手)"] = $"{others}/wnzmb.png",
                ["#(sinar拥抱)"] = $"{others}/hug.png",
                ["#(sinar跪了)"] = $"{others}/fuley.png",
                ["#(sinar菜鸡)"] = $"{others}/bad.png",
                ["#(sinar抱拳)"] = $"{others}/baoquan.png",
                ["#(sinar拳头)"] = $"{others}/comeon.png",
                ["#(sinar很棒)"] = $"{others}/good.png",
                ["#(sinar点赞)"] = $"{others}/good2.png",
                ["#(sinar勾引)"] = $"{others}/gouyin.png",
                ["#(sinar喜欢)"] = $"{others}/like2.png",
                ["#(sinar不不不)"] = $"{others}/nonono.png",
                ["#(sinarOK)"] = $"{others}/ok.png",
                ["#(sinar握手)"] = $"{others}/shandup.png",
                ["#(sinar加油)"] = $"{others}/strong.png",
                ["#(sinar胜利)"] = $"{others}/yeah2.png",
                ["#(sinar礼物)"] = $"{others}/bbox.png",
                ["#(sinar蛋糕)"] = $"{others}/cake.png",
                ["#(sinar照相机)"] = $"{others}/camera.png",
                ["#(sinar干杯)"] = $"{others}/cheers.png",
                ["#(sinar灯笼)"] = $"{others}/denglong.png",
                ["#(sinar给力)"] = $"{others}/geili.png",
                ["#(sinar蜡烛)"] = $"{others}/lazhu.png",
                ["#(sinar麦克风)"] = $"{others}/microphone.png",
                ["#(sinar音乐)"] = $"{others}/music.png",
                ["#(sinar灰机)"] = $"{others}/plane.png",
                ["#(sinar点赞)"] = $"{others}/zan.png",
                ["#(sinar奥特曼)"] = $"{others}/untraman.png",
                ["#(sinar团队)"] = $"{others}/together.png",
                ["#(sinar双喜)"] = $"{others}/shuangxi.png",
                ["#(sinar新狼)"] = $"{others}/wboy.png",
                ["#(sinar新娘)"] = $"{others}/wgirl.png",
                ["#(sinar多云)"] = $"{others}/cloudy.png",
                ["#(sinar多云有雨)"] = $"{others}/dcloud.png",
                ["#(sinar绿了)"] = $"{others}/greeny.png",
                ["#(sinar下雨)"] = $"{others}/rainy.png",
                ["#(sinar玫瑰)"] = $"{others}/rose.png",
                ["#(sinar太阳)"] = $"{others}/sun.png",
                ["#(sinar晚上)"] = $"{others}/night.png",
                ["#(sinar爱心)"] = $"{others}/heart.png",
                ["#(sinar心碎)"] = $"{others}/heartbroken.png",
                ["#(sinarBSN)"] = $"{others}/bishi.png",
            };
        }

    }
}
