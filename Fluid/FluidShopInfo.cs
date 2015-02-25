using Fluid.Blocks;
using System.Collections.Generic;

namespace Fluid
{
    public class FluidShopInfo
    {
        private Dictionary<string, List<int>> m_BlockPacks = new Dictionary<string, List<int>>();
        private Dictionary<int, string> m_Smilies = new Dictionary<int, string>();

        /// <summary>
        /// Gets a list of default blocks
        /// </summary>
        public int[] GetDefaultBlocks()
        {
            return new int[] { 9, 10, 11, 12, 13, 14, 15, 182, 16, 17, 18, 19, 20, 21, 29, 30, 31, 34, 35, 36, 22, 32, 33, 0, 1, 2, 3, 4, 6, 7, 8, 26, 27, 28, 23, 24, 25, 100, 101, 5, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 411, 412, 413, 414, 1005, 1006, 1007, 1008, 1009, 1010, 408, 409, 410, 1018, 1022, 1023, 1024, 644, 645, 646, 647, 648 };
        }

        /// <summary>
        /// Gets the corresponding block pack to a block id
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <returns>The corresponding block pack if found; otherwise null</returns>
        public string GetBlockPack(BlockID blockId)
        {
            foreach (KeyValuePair<string, List<int>> blockPack in m_BlockPacks)
            {
                if (blockPack.Value.Contains((int)blockId))
                {
                    return blockPack.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the smiley shop id
        /// </summary>
        /// <param name="smiley">The smiley</param>
        public string GetSmileyId(FaceID smiley)
        {
            int id = (int)smiley;
            if (m_Smilies.ContainsKey(id))
            {
                return m_Smilies[id];
            }

            return null;
        }

        public FluidShopInfo()
        {
            m_Smilies.Add(0, "");
            m_Smilies.Add(1, "");
            m_Smilies.Add(2, "");
            m_Smilies.Add(3, "");
            m_Smilies.Add(4, "");
            m_Smilies.Add(5, "");
            m_Smilies.Add(18, "");
            m_Smilies.Add(6, "pro");
            m_Smilies.Add(7, "pro");
            m_Smilies.Add(8, "pro");
            m_Smilies.Add(9,  "pro");
            m_Smilies.Add(10, "pro");
            m_Smilies.Add(11, "pro");
            m_Smilies.Add(12, "smileyninja");
            m_Smilies.Add(13, "smileysanta");
            m_Smilies.Add(14, "smileyworker");
            m_Smilies.Add(15, "smileybigspender");
            m_Smilies.Add(16, "smileysuper");
            m_Smilies.Add(17, "smileysupprice");         
            m_Smilies.Add(19, "smileygirl");
            m_Smilies.Add(20, "mixednewyear2010");
            m_Smilies.Add(21, "smileycoy");
            m_Smilies.Add(22, "smileywizard");
            m_Smilies.Add(23, "smileyfanboy");
            m_Smilies.Add(24, "smileyterminator");
            m_Smilies.Add(25, "smileyxd");
            m_Smilies.Add(26, "smileybully");
            m_Smilies.Add(27, "smileycommando");
            m_Smilies.Add(28, "smileyvalentines2011");
            m_Smilies.Add(29, "smileybird");
            m_Smilies.Add(30, "smileybunni");
            m_Smilies.Add(31, "unobtainable");
            m_Smilies.Add(32, "smileywizard2");
            m_Smilies.Add(33, "smileyxdp");
            m_Smilies.Add(34, "smileypostman");
            m_Smilies.Add(35, "smileytemplar");
            m_Smilies.Add(36, "smileyangel");
            m_Smilies.Add(37, "smileynurse");
            m_Smilies.Add(38, "smileyhw2011vampire");
            m_Smilies.Add(39, "smileyhw2011ghost");
            m_Smilies.Add(40, "smileyhw2011frankenstein");
            m_Smilies.Add(41, "smileywitch");
            m_Smilies.Add(42, "smileytg2011indian");
            m_Smilies.Add(43, "smileytg2011pilgrim");
            m_Smilies.Add(44, "smileypumpkin1");
            m_Smilies.Add(45, "smileypumpkin2");
            m_Smilies.Add(46, "smileyxmassnowman");
            m_Smilies.Add(47, "smileyxmasreindeer");
            m_Smilies.Add(48, "smileyxmasgrinch");
            m_Smilies.Add(49, "bricknode");
            m_Smilies.Add(50, "brickdrums");
            m_Smilies.Add(51, "smileysigh");
            m_Smilies.Add(52, "smileyrobber");
            m_Smilies.Add(53, "smileypolice");
            m_Smilies.Add(54, "smileypurpleghost");
            m_Smilies.Add(55, "smileypirate");
            m_Smilies.Add(56, "smileyviking");
            m_Smilies.Add(57, "smileykarate");
            m_Smilies.Add(58, "smileycowboy");
            m_Smilies.Add(59, "smileydiver");
            m_Smilies.Add(60, "smileytanned");
            m_Smilies.Add(61, "smileypropeller");
            m_Smilies.Add(62, "smileyhardhat");
            m_Smilies.Add(63, "smileygasmask");
            m_Smilies.Add(64, "smileyrobot");
            m_Smilies.Add(65, "smileypeasant");
            m_Smilies.Add(66, "smileysoldier");
            m_Smilies.Add(67, "smileyblacksmith");
            m_Smilies.Add(68, "smileylaughing");
            m_Smilies.Add(69, "smileylaika");
            m_Smilies.Add(70, "smileyalien");
            m_Smilies.Add(71, "smileyastronaut");
            m_Smilies.Add(72, "unobtainable");
            m_Smilies.Add(73, "unobtainable");
            m_Smilies.Add(74, "unobtainable");
            m_Smilies.Add(75, "unobtainable");
            m_Smilies.Add(76, "smileycannon");
            m_Smilies.Add(77, "smileymonster");
            m_Smilies.Add(78, "smileyskeleton");
            m_Smilies.Add(79, "smileymadscientist");
            m_Smilies.Add(80, "smileyheadhunter");
            m_Smilies.Add(81, "smileysafari");
            m_Smilies.Add(82, "smileyarchaeologist");
            m_Smilies.Add(83, "smileynewyear2012");
            m_Smilies.Add(84, "smileywinter");
            m_Smilies.Add(85, "smileyfiredeamon");
            m_Smilies.Add(86, "smileybishop");
            m_Smilies.Add(87, "unobtainable");
            m_Smilies.Add(88, "smileyzombieslayer");
            m_Smilies.Add(89, "smileyunit");
            m_Smilies.Add(90, "smileyspartan");
            m_Smilies.Add(91, "smileyhelen");
            m_Smilies.Add(92, "smileycow");
            m_Smilies.Add(93, "smileyscarecrow");
            m_Smilies.Add(94, "smileydarkwizard");
            m_Smilies.Add(95, "smileykungfumaster");
            m_Smilies.Add(96, "smileyfox");
            m_Smilies.Add(97, "smileynightvision");
            m_Smilies.Add(98, "smileysummergirl");
            m_Smilies.Add(99, "smileyfanboy2");
            m_Smilies.Add(100, "unobtainable");
            m_Smilies.Add(101, "smileygingerbread");
            m_Smilies.Add(102, "smileycaroler");
            m_Smilies.Add(103, "smileyelf");
            m_Smilies.Add(104, "smileynutcracker");
            m_Smilies.Add(105, "brickvalentines2015");

            m_BlockPacks.Add("", new List<int>(new int[] { 9, 10, 11, 12, 13, 14, 15, 182 }));
            m_BlockPacks.Add("pro", new List<int>(new int[] { 37, 38, 39, 40, 41, 42 }));        
            m_BlockPacks.Add("brickblackblock", new List<int>(new int[] { 44 }));
            m_BlockPacks.Add("brickfactorypack", new List<int>(new int[] { 45, 46, 47, 48, 49 }));
            m_BlockPacks.Add("bricksecret", new List<int>(new int[] { 50, 243 }));
            m_BlockPacks.Add("brickglass", new List<int>(new int[] { 51, 52, 53, 54, 55, 56, 57, 58 }));
            m_BlockPacks.Add("brickminiral", new List<int>(new int[] { 70, 71, 72, 73, 74, 75, 76 }));
            m_BlockPacks.Add("brickxmas2011", new List<int>(new int[] { 78, 79, 80, 81, 82, 218, 219, 220, 221, 222 }));
            m_BlockPacks.Add("brickswitchpurple", new List<int>(new int[] { 113, 184, 185 }));
            m_BlockPacks.Add("brickcoingate", new List<int>(new int[] { 165 }));
            m_BlockPacks.Add("bricktimeddoor", new List<int>(new int[] { 157, 156 }));
            m_BlockPacks.Add("buildersclub", new List<int>(new int[] { 200, 201 }));
            m_BlockPacks.Add("brickzombiedoor", new List<int>(new int[] { 206, 207 }));
            m_BlockPacks.Add("brickcoindoor", new List<int>(new int[] { 43 }));
            m_BlockPacks.Add("brickboost", new List<int>(new int[] { 114, 115, 116, 117 }));
            m_BlockPacks.Add("bricknode", new List<int>(new int[] { 77 }));
            m_BlockPacks.Add("brickdrums", new List<int>(new int[] { 83 }));
            m_BlockPacks.Add("hidden", new List<int>(new int[] { 110, 111 }));
            m_BlockPacks.Add("brickspawn", new List<int>(new int[] { 255 }));
            m_BlockPacks.Add("brickcheckpoint", new List<int>(new int[] { 360 }));
            m_BlockPacks.Add("brickcomplete", new List<int>(new int[] { 121 }));
            m_BlockPacks.Add("brickspike", new List<int>(new int[] { 361 }));
            m_BlockPacks.Add("brickfire", new List<int>(new int[] { 368 }));
            m_BlockPacks.Add("brickcastle", new List<int>(new int[] { 118, 158, 159, 160, 599, 325, 326 }));
            m_BlockPacks.Add("brickninja", new List<int>(new int[] { 120, 96, 97, 564, 565, 566, 567, 276, 277, 278, 279, 280, 281, 282, 283, 284 }));
            m_BlockPacks.Add("brickjungle", new List<int>(new int[] { 98, 99, 199, 621, 622, 623, 357, 358, 359 }));
            m_BlockPacks.Add("brickwater", new List<int>(new int[] { 119, 300, 574, 575, 576, 577, 578 }));
            m_BlockPacks.Add("brickswamp", new List<int>(new int[] { 369, 370, 630, 371, 372, 373 }));
            m_BlockPacks.Add("brickinvisibleportal", new List<int>(new int[] { 381 }));
            m_BlockPacks.Add("brickportal", new List<int>(new int[] { 242 }));
            m_BlockPacks.Add("brickworldportal", new List<int>(new int[] { 374 }));
            m_BlockPacks.Add("brickdiamond", new List<int>(new int[] { 241 }));
            m_BlockPacks.Add("brickcake", new List<int>(new int[] { 337 }));
            m_BlockPacks.Add("brickchristmas2010", new List<int>(new int[] { 249, 250, 251, 252, 253, 254 }));
            m_BlockPacks.Add("mixednewyear2010", new List<int>(new int[] { 244, 245, 246, 247, 248 }));
            m_BlockPacks.Add("brickspring2011", new List<int>(new int[] { 233, 234, 235, 236, 237, 238, 239, 240 }));
            m_BlockPacks.Add("brickhwtrophy", new List<int>(new int[] { 223 }));
            m_BlockPacks.Add("brickeaster2012", new List<int>(new int[] { 256, 257, 258, 259, 260 }));
            m_BlockPacks.Add("brickbgchecker", new List<int>(new int[] { 513, 514, 515, 516, 517, 518, 519 }));
            m_BlockPacks.Add("brickbgdark", new List<int>(new int[] { 520, 521, 522, 523, 524, 525, 526 }));
            m_BlockPacks.Add("brickbgnormal", new List<int>(new int[] { 610, 611, 612, 613, 614, 615, 616 }));
            m_BlockPacks.Add("brickbgpastel", new List<int>(new int[] { 527, 528, 529, 530, 531, 532 }));
            m_BlockPacks.Add("brickbgcanvas", new List<int>(new int[] { 533, 534, 535, 536, 537, 538 }));
            m_BlockPacks.Add("brickbgcarnival", new List<int>(new int[] { 545, 546, 547, 548, 549 }));
            m_BlockPacks.Add("brickcandy", new List<int>(new int[] { 60, 61, 62, 63, 64, 65, 66, 67, 227, 539, 540 }));
            m_BlockPacks.Add("bricksummer2011", new List<int>(new int[] { 59, 228, 229, 230, 231, 232 }));
            m_BlockPacks.Add("brickhw2011", new List<int>(new int[] { 68, 69, 224, 225, 226, 541, 542, 543, 544 }));
            m_BlockPacks.Add("brickscifi", new List<int>(new int[] { 84, 85, 86, 87, 88, 89, 90, 91 }));
            m_BlockPacks.Add("brickprison", new List<int>(new int[] { 261, 92, 550, 551, 552, 553 }));
            m_BlockPacks.Add("brickwindow", new List<int>(new int[] { 262, 263, 264, 265, 266, 267, 268, 269, 270 }));
            m_BlockPacks.Add("brickpirate", new List<int>(new int[] { 93, 94, 271, 272, 554, 555, 556, 557, 558, 559, 560 }));
            m_BlockPacks.Add("brickviking", new List<int>(new int[] { 95, 273, 274, 275, 561, 562, 563 }));
            m_BlockPacks.Add("brickcowboy", new List<int>(new int[] { 122, 123, 124, 125, 126, 127, 568, 569, 570, 571, 572, 573, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299 }));
            m_BlockPacks.Add("brickplastic", new List<int>(new int[] { 128, 129, 130, 131, 132, 133, 134, 135 }));
            m_BlockPacks.Add("bricksand", new List<int>(new int[] { 137, 138, 139, 140, 141, 142, 579, 580, 581, 582, 583, 584, 301, 302, 303, 304, 305, 306 }));
            m_BlockPacks.Add("bricksummer2012", new List<int>(new int[] { 307, 308, 309, 310 }));
            m_BlockPacks.Add("brickcloud", new List<int>(new int[] { 143, 311, 312, 313, 314, 315, 316, 317, 318 }));
            m_BlockPacks.Add("brickplateiron", new List<int>(new int[] { 144, 145, 585, 586, 587, 588, 589 }));
            m_BlockPacks.Add("bricksigns", new List<int>(new int[] { 319, 320, 321, 322, 323, 324 }));
            m_BlockPacks.Add("brickindustrial", new List<int>(new int[] { 146, 147, 148, 149, 150, 151, 152, 153 }));
            m_BlockPacks.Add("bricktimbered", new List<int>(new int[] { 154, 590, 591, 592, 593, 594, 595, 596, 597, 598 }));
            m_BlockPacks.Add("brickmedieval", new List<int>(new int[] { 162, 163, 327, 328, 329, 330, 331, 600 }));
            m_BlockPacks.Add("brickpipe", new List<int>(new int[] { 166, 167, 168, 169, 170, 171 }));
            m_BlockPacks.Add("brickrocket", new List<int>(new int[] { 172, 173, 174, 175, 601, 602, 603, 604, 332, 333, 334, 335 }));
            m_BlockPacks.Add("brickmars", new List<int>(new int[] { 176, 177, 178, 179, 180, 181, 605, 606, 607, 336 }));
            m_BlockPacks.Add("brickmonster", new List<int>(new int[] { 608, 609, 338, 339, 340, 341, 342 }));
            m_BlockPacks.Add("brickfog", new List<int>(new int[] { 343, 344, 345, 346, 347, 348, 349, 350, 351 }));
            m_BlockPacks.Add("brickhw2012", new List<int>(new int[] { 352, 353, 354, 355, 356 }));
            m_BlockPacks.Add("brickchecker", new List<int>(new int[] { 186, 187, 188, 189, 190, 191, 192 }));
            m_BlockPacks.Add("brickjungleruins", new List<int>(new int[] { 193, 194, 195, 196, 197, 198, 617, 618, 619, 620 }));
            m_BlockPacks.Add("brickxmas2012", new List<int>(new int[] { 624, 625, 626, 362, 363, 364, 365, 366, 367 }));
            m_BlockPacks.Add("bricklava", new List<int>(new int[] { 202, 203, 204, 627, 628, 629 }));
            m_BlockPacks.Add("brickscifi2013", new List<int>(new int[] { 375, 376, 377, 378, 379, 380, 637 }));
            m_BlockPacks.Add("bricksparta", new List<int>(new int[] { 382, 383, 384, 208, 209, 210, 211, 638, 639, 640 }));
            m_BlockPacks.Add("bricksign", new List<int>(new int[] { 385 }));
            m_BlockPacks.Add("brickfarm", new List<int>(new int[] { 386, 387, 388, 389, 212 }));
            m_BlockPacks.Add("brickoneway", new List<int>(new int[] { 1001, 1002, 1003, 1004 }));
            m_BlockPacks.Add("brickautumn2014", new List<int>(new int[] { 390, 391, 392, 393, 394, 395, 396, 641, 642, 643 }));
            m_BlockPacks.Add("brickvalentines2015", new List<int>(new int[] { 405, 406, 407 }));
            m_BlockPacks.Add("brickhologram", new List<int>(new int[] { 397 }));
            m_BlockPacks.Add("brickdeathdoor", new List<int>(new int[] { 1011 }));
            m_BlockPacks.Add("brickdeathgate", new List<int>(new int[] { 1012 }));
            m_BlockPacks.Add("bricktimedgate", new List<int>(new int[] { 157 }));
            m_BlockPacks.Add("brickbluecoindoor", new List<int>(new int[] { 213 }));
            m_BlockPacks.Add("brickbluecoingate", new List<int>(new int[] { 214 }));
        }
    }
}
