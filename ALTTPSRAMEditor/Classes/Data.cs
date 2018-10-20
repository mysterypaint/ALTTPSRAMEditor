using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    class Data
    {
        public Data(Dictionary<char, int> enChar, Dictionary<char, int> jpChar, Dictionary<UInt16, char> rawENChar, Dictionary<UInt16, char> rawJPChar)
        {
            // Define Font lookup tables
            // English
            enChar.Add(' ', 0);
            for (int i = 65; i <= 90; i++) enChar.Add((char)i, i - 64);
            for (int i = 97; i <= 122; i++) enChar.Add((char)i, i - 69);
            enChar.Add('-', 54);
            enChar.Add('.', 55);
            enChar.Add(',', 56);
            for (int i = 48; i <= 57; i++) enChar.Add((char)i, i + 11);
            enChar.Add('!', 69);
            enChar.Add('?', 70);
            enChar.Add('(', 71);
            enChar.Add(')', 72);

            // Japanese
            jpChar.Add('　', 1); // Space

            jpChar.Add('あ', 5); // a
            jpChar.Add('い', 6); // i
            jpChar.Add('う', 7); // u
            jpChar.Add('え', 8); // e
            jpChar.Add('お', 9); // o

            jpChar.Add('か', 10); // ka
            jpChar.Add('き', 11); // ki
            jpChar.Add('く', 12); // ku
            jpChar.Add('け', 13); // ke
            jpChar.Add('こ', 14); // ko

            jpChar.Add('さ', 15); // sa
            jpChar.Add('し', 16); // shi
            jpChar.Add('す', 17); // su
            jpChar.Add('せ', 18); // se
            jpChar.Add('そ', 19); // so

            jpChar.Add('た', 20); // ta
            jpChar.Add('ち', 21); // chi
            jpChar.Add('つ', 22); // tsu
            jpChar.Add('て', 23); // te
            jpChar.Add('と', 24); // to

            jpChar.Add('な', 25); // na
            jpChar.Add('に', 26); // ni
            jpChar.Add('ぬ', 27); // nu
            jpChar.Add('ね', 28); // ne
            jpChar.Add('の', 29); // no

            jpChar.Add('は', 30); // ha
            jpChar.Add('ひ', 31); // hi
            jpChar.Add('ふ', 32); // fu
            jpChar.Add('へ', 33); // he
            jpChar.Add('ほ', 34); // ho

            jpChar.Add('ま', 35); // ma
            jpChar.Add('み', 36); // mi
            jpChar.Add('む', 37); // mu
            jpChar.Add('め', 38); // me
            jpChar.Add('も', 39); // mo

            jpChar.Add('や', 40); // ya
            jpChar.Add('ゆ', 42); // yu
            jpChar.Add('よ', 44); // yo

            jpChar.Add('ら', 45); // ra
            jpChar.Add('り', 46); // ri
            jpChar.Add('る', 47); // ru
            jpChar.Add('れ', 48); // re
            jpChar.Add('ろ', 49); // ro

            jpChar.Add('わ', 50); // wa
            jpChar.Add('を', 51); // wo
            jpChar.Add('ん', 52); // n

            jpChar.Add('ー', 54);

            jpChar.Add('が', 55); // ga
            jpChar.Add('ぎ', 56); // gi
            jpChar.Add('ぐ', 57); // gu
            jpChar.Add('げ', 58); // ge
            jpChar.Add('ご', 59); // go

            jpChar.Add('ざ', 60); // za
            jpChar.Add('じ', 61); // ji
            jpChar.Add('ず', 62); // zu
            jpChar.Add('ぜ', 63); // ze
            jpChar.Add('ぞ', 64); // zo

            jpChar.Add('だ', 65); // da
            jpChar.Add('ぢ', 66); // di
            jpChar.Add('づ', 67); // du
            jpChar.Add('で', 68); // de
            jpChar.Add('ど', 69); // do

            jpChar.Add('ば', 70); // ba
            jpChar.Add('び', 71); // bi
            jpChar.Add('ぶ', 72); // bu
            jpChar.Add('べ', 73); // be
            jpChar.Add('ぼ', 74); // bo

            jpChar.Add('ぱ', 75); // pa
            jpChar.Add('ぴ', 76); // pi
            jpChar.Add('ぷ', 77); // pu
            jpChar.Add('ぺ', 78); // pe
            jpChar.Add('ぽ', 79); // po

            jpChar.Add('ぁ', 80); // ~a
            jpChar.Add('ぃ', 81); // ~i
            jpChar.Add('ぅ', 82); // ~u
            jpChar.Add('ぇ', 83); // ~e
            jpChar.Add('ぉ', 84); // ~o

            jpChar.Add('ゃ', 85); // ~ya
            jpChar.Add('ゅ', 86); // ~yu
            jpChar.Add('ょ', 87); // ~yo

            jpChar.Add('っ', 89); // ~tsu

            // Katakana
            jpChar.Add('ア', 95); // a
            jpChar.Add('イ', 96); // i
            jpChar.Add('ウ', 97); // u
            jpChar.Add('エ', 98); // e
            jpChar.Add('オ', 99); // o

            jpChar.Add('カ', 100); // ka
            jpChar.Add('キ', 101); // ki
            jpChar.Add('ク', 102); // ku
            jpChar.Add('ケ', 103); // ke
            jpChar.Add('コ', 104); // ko

            jpChar.Add('サ', 105); // sa
            jpChar.Add('シ', 106); // shi
            jpChar.Add('ス', 107); // su
            jpChar.Add('セ', 108); // se
            jpChar.Add('ソ', 109); // so

            jpChar.Add('タ', 110); // ta
            jpChar.Add('チ', 111); // chi
            jpChar.Add('ツ', 112); // tsu
            jpChar.Add('テ', 113); // te
            jpChar.Add('ト', 114); // to

            jpChar.Add('ナ', 115); // na
            jpChar.Add('ニ', 116); // ni
            jpChar.Add('ヌ', 117); // nu
            jpChar.Add('ネ', 118); // ne
            jpChar.Add('ノ', 119); // no

            jpChar.Add('ハ', 120); // ha
            jpChar.Add('ヒ', 121); // hi
            jpChar.Add('フ', 122); // fu
            jpChar.Add('ヘ', 123); // he
            jpChar.Add('ホ', 124); // ho

            jpChar.Add('マ', 125); // ma
            jpChar.Add('ミ', 126); // mi
            jpChar.Add('ム', 127); // mu
            jpChar.Add('メ', 128); // me
            jpChar.Add('モ', 129); // mo

            jpChar.Add('ヤ', 130); // ya
            jpChar.Add('ユ', 132); // yu
            jpChar.Add('ヨ', 134); // yo

            jpChar.Add('ラ', 135); // ra
            jpChar.Add('リ', 136); // ri
            jpChar.Add('ル', 137); // ru
            jpChar.Add('レ', 138); // re
            jpChar.Add('ロ', 139); // ro

            jpChar.Add('ワ', 140); // wa
            jpChar.Add('ヲ', 141); // wo
            jpChar.Add('ン', 142); // n

            jpChar.Add('－', 144);

            jpChar.Add('ガ', 145); // ga
            jpChar.Add('ギ', 146); // gi
            jpChar.Add('グ', 147); // gu
            jpChar.Add('ゲ', 148); // ge
            jpChar.Add('ゴ', 149); // go

            jpChar.Add('ザ', 150); // za
            jpChar.Add('ジ', 151); // ji
            jpChar.Add('ズ', 152); // zu
            jpChar.Add('ゼ', 153); // ze
            jpChar.Add('ゾ', 154); // zo

            jpChar.Add('ダ', 155); // da
            jpChar.Add('ヂ', 156); // di
            jpChar.Add('ヅ', 157); // du
            jpChar.Add('デ', 158); // de
            jpChar.Add('ド', 159); // do

            jpChar.Add('バ', 160); // ba
            jpChar.Add('ビ', 161); // bi
            jpChar.Add('ブ', 162); // bu
            jpChar.Add('ベ', 163); // be
            jpChar.Add('ボ', 164); // bo

            jpChar.Add('パ', 165); // pa
            jpChar.Add('ピ', 166); // pi
            jpChar.Add('プ', 167); // pu
            jpChar.Add('ペ', 168); // pe
            jpChar.Add('ポ', 169); // po

            jpChar.Add('ァ', 170); // ~a
            jpChar.Add('ィ', 171); // ~i
            jpChar.Add('ゥ', 172); // ~u
            jpChar.Add('ェ', 173); // ~e
            jpChar.Add('ォ', 174); // ~o

            jpChar.Add('ャ', 175); // ~ya
            jpChar.Add('ュ', 176); // ~yu
            jpChar.Add('ョ', 177); // ~yo

            jpChar.Add('ッ', 179); // ~tsu

            // Romaji
            jpChar.Add('A', 180);
            jpChar.Add('B', 181);
            jpChar.Add('C', 182);
            jpChar.Add('D', 183);
            jpChar.Add('E', 184);
            jpChar.Add('F', 185);
            jpChar.Add('G', 186);
            jpChar.Add('H', 187);
            jpChar.Add('I', 188);
            jpChar.Add('J', 189);
            jpChar.Add('K', 190);
            jpChar.Add('L', 191);
            jpChar.Add('M', 192);
            jpChar.Add('N', 193);
            jpChar.Add('O', 194);
            jpChar.Add('P', 195);
            jpChar.Add('Q', 196);
            jpChar.Add('R', 197);
            jpChar.Add('S', 198);
            jpChar.Add('T', 199);
            jpChar.Add('U', 200);
            jpChar.Add('V', 201);
            jpChar.Add('W', 202);
            jpChar.Add('X', 203);
            jpChar.Add('Y', 204);
            jpChar.Add('Z', 205);
            jpChar.Add('-', 206);
            jpChar.Add('~', 207);

            // Raw in-game hex chars
            // English
            rawENChar.Add(0xA9, ' ');
            rawENChar.Add(0x00, 'A');
            rawENChar.Add(0x01, 'B');
            rawENChar.Add(0x02, 'C');
            rawENChar.Add(0x03, 'D');
            rawENChar.Add(0x04, 'E');
            rawENChar.Add(0x05, 'F');
            rawENChar.Add(0x06, 'G');
            rawENChar.Add(0x07, 'H');
            rawENChar.Add(0x08, 'I');
            rawENChar.Add(0x09, 'J');
            rawENChar.Add(0x0A, 'K');
            rawENChar.Add(0x0B, 'L');
            rawENChar.Add(0x0C, 'M');
            rawENChar.Add(0x0D, 'N');
            rawENChar.Add(0x0E, 'O');
            rawENChar.Add(0x0F, 'P');
            rawENChar.Add(0x10, '%'); // Glitched ? char, use 0x6F instead
            rawENChar.Add(0x20, 'Q');
            rawENChar.Add(0x21, 'R');
            rawENChar.Add(0x22, 'S');
            rawENChar.Add(0x23, 'T');
            rawENChar.Add(0x24, 'U');
            rawENChar.Add(0x25, 'V');
            rawENChar.Add(0x26, 'W');
            rawENChar.Add(0x27, 'X');
            rawENChar.Add(0x28, 'Y');
            rawENChar.Add(0x29, 'Z');
            rawENChar.Add(0x2A, 'a');
            rawENChar.Add(0x2B, 'b');
            rawENChar.Add(0x2C, 'c');
            rawENChar.Add(0x2D, 'd');
            rawENChar.Add(0x2E, 'e');
            rawENChar.Add(0x2F, 'f');
            rawENChar.Add(0x40, 'g');
            rawENChar.Add(0x41, 'h');
            rawENChar.Add(0x42, 'k');
            rawENChar.Add(0x43, 'j');
            rawENChar.Add(0xC0, 'i');
            rawENChar.Add(0x45, 'l');
            rawENChar.Add(0x46, 'm');
            rawENChar.Add(0x47, 'n');
            rawENChar.Add(0x48, 'o');
            rawENChar.Add(0x49, 'p');
            rawENChar.Add(0x4A, 'q');
            rawENChar.Add(0x4B, 'r');
            rawENChar.Add(0x4C, 's');
            rawENChar.Add(0x4D, 't');
            rawENChar.Add(0x4E, 'u');
            rawENChar.Add(0x4F, 'v');
            rawENChar.Add(0x60, 'w');
            rawENChar.Add(0x61, 'x');
            rawENChar.Add(0x62, 'y');
            rawENChar.Add(0x63, 'z');
            rawENChar.Add(0x64, '0');
            rawENChar.Add(0x65, '1');
            rawENChar.Add(0x66, '2');
            rawENChar.Add(0x67, '3');
            rawENChar.Add(0x68, '4');
            rawENChar.Add(0x69, '5');
            rawENChar.Add(0x6A, '6');
            rawENChar.Add(0x6B, '7');
            rawENChar.Add(0x6C, '8');
            rawENChar.Add(0x6D, '9');
            rawENChar.Add(0x6F, '?');
            rawENChar.Add(0xC1, '!');
            rawENChar.Add(0x80, '-');
            rawENChar.Add(0x81, '.');
            rawENChar.Add(0x82, ',');
            rawENChar.Add(0x85, '(');
            rawENChar.Add(0x86, ')');

            // Japanese
            // Hiragana
            rawJPChar.Add(0x18C, ' ');
            rawJPChar.Add(0x0, 'あ'); // a
            rawJPChar.Add(0x1, 'い'); // i
            rawJPChar.Add(0x2, 'う'); // u
            rawJPChar.Add(0x3, 'え'); // e
            rawJPChar.Add(0x4, 'お'); // o

            rawJPChar.Add(0x8, 'か'); // ka
            rawJPChar.Add(0x9, 'き'); // ki
            rawJPChar.Add(0xA, 'く'); // ku
            rawJPChar.Add(0xB, 'け'); // ke
            rawJPChar.Add(0xC, 'こ'); // ko

            rawJPChar.Add(0x20, 'さ'); // sa
            rawJPChar.Add(0x21, 'し'); // shi
            rawJPChar.Add(0x22, 'す'); // su
            rawJPChar.Add(0x23, 'せ'); // se
            rawJPChar.Add(0x24, 'そ'); // so

            rawJPChar.Add(0x28, 'た'); // ta
            rawJPChar.Add(0x29, 'ち'); // chi
            rawJPChar.Add(0x2A, 'つ'); // tsu
            rawJPChar.Add(0x2B, 'て'); // te
            rawJPChar.Add(0x2C, 'と'); // to

            rawJPChar.Add(0x40, 'な'); // na
            rawJPChar.Add(0x41, 'に'); // ni
            rawJPChar.Add(0x42, 'ぬ'); // nu
            rawJPChar.Add(0x43, 'ね'); // ne
            rawJPChar.Add(0x44, 'の'); // no

            rawJPChar.Add(0x48, 'は'); // ha
            rawJPChar.Add(0x49, 'ひ'); // hi
            rawJPChar.Add(0x4A, 'ふ'); // fu
            rawJPChar.Add(0x4B, 'へ'); // he
            rawJPChar.Add(0x4C, 'ほ'); // ho

            rawJPChar.Add(0x60, 'ま'); // ma
            rawJPChar.Add(0x61, 'み'); // mi
            rawJPChar.Add(0x62, 'む'); // mu
            rawJPChar.Add(0x63, 'め'); // me
            rawJPChar.Add(0x64, 'も'); // mo

            rawJPChar.Add(0x5, 'や'); // ya
            rawJPChar.Add(0x6, 'ゆ'); // yu
            rawJPChar.Add(0x7, 'よ'); // yo

            rawJPChar.Add(0x68, 'ら'); // ra
            rawJPChar.Add(0x69, 'り'); // ri
            rawJPChar.Add(0x6A, 'る'); // ru
            rawJPChar.Add(0x6B, 'れ'); // re
            rawJPChar.Add(0x6C, 'ろ'); // ro

            rawJPChar.Add(0xD, 'わ'); // wa
            rawJPChar.Add(0xE, 'を'); // wo
            rawJPChar.Add(0xF, 'ん'); // n

            rawJPChar.Add(0x189, 'ー');

            rawJPChar.Add(0x25, 'が'); // ga
            rawJPChar.Add(0x26, 'ぎ'); // gi
            rawJPChar.Add(0x27, 'ぐ'); // gu
            rawJPChar.Add(0x2D, 'げ'); // ge
            rawJPChar.Add(0x2E, 'ご'); // go

            rawJPChar.Add(0x2F, 'ざ'); // za
            rawJPChar.Add(0x45, 'じ'); // ji
            rawJPChar.Add(0x46, 'ず'); // zu
            rawJPChar.Add(0x47, 'ぜ'); // ze
            rawJPChar.Add(0x4D, 'ぞ'); // zo

            rawJPChar.Add(0x4E, 'だ'); // da
            rawJPChar.Add(0x4F, 'ぢ'); // di
            rawJPChar.Add(0x65, 'づ'); // du
            rawJPChar.Add(0x66, 'で'); // de
            rawJPChar.Add(0x67, 'ど'); // do

            rawJPChar.Add(0x6D, 'ば'); // ba
            rawJPChar.Add(0x6E, 'び'); // bi
            rawJPChar.Add(0x6F, 'ぶ'); // bu
            rawJPChar.Add(0x80, 'べ'); // be
            rawJPChar.Add(0x81, 'ぼ'); // bo

            rawJPChar.Add(0x82, 'ぱ'); // pa
            rawJPChar.Add(0x83, 'ぴ'); // pi
            rawJPChar.Add(0x84, 'ぷ'); // pu
            rawJPChar.Add(0x85, 'ぺ'); // pe
            rawJPChar.Add(0x86, 'ぽ'); // po

            rawJPChar.Add(0x8B, 'ぁ'); // ~a
            rawJPChar.Add(0x8C, 'ぃ'); // ~i
            rawJPChar.Add(0x8D, 'ぅ'); // ~u
            rawJPChar.Add(0x8E, 'ぇ'); // ~e
            rawJPChar.Add(0x8F, 'ぉ'); // ~o

            rawJPChar.Add(0x87, 'ゃ'); // ~ya
            rawJPChar.Add(0x88, 'ゅ'); // ~yu
            rawJPChar.Add(0x89, 'ょ'); // ~yo

            rawJPChar.Add(0x8A, 'っ'); // ~tsu

            // Katakana
            rawJPChar.Add(0xA0, 'ア'); // a
            rawJPChar.Add(0xA1, 'イ'); // i
            rawJPChar.Add(0xA2, 'ウ'); // u
            rawJPChar.Add(0xA3, 'エ'); // e
            rawJPChar.Add(0xA4, 'オ'); // o

            rawJPChar.Add(0xA8, 'カ'); // ka
            rawJPChar.Add(0xA9, 'キ'); // ki
            rawJPChar.Add(0xAA, 'ク'); // ku
            rawJPChar.Add(0xAB, 'ケ'); // ke
            rawJPChar.Add(0xAC, 'コ'); // ko

            rawJPChar.Add(0xC0, 'サ'); // sa
            rawJPChar.Add(0xC1, 'シ'); // shi
            rawJPChar.Add(0xC2, 'ス'); // su
            rawJPChar.Add(0xC3, 'セ'); // se
            rawJPChar.Add(0xC4, 'ソ'); // so

            rawJPChar.Add(0xC8, 'タ'); // ta
            rawJPChar.Add(0xC9, 'チ'); // chi
            rawJPChar.Add(0xCA, 'ツ'); // tsu
            rawJPChar.Add(0xCB, 'テ'); // te
            rawJPChar.Add(0xCC, 'ト'); // to

            rawJPChar.Add(0xE0, 'ナ'); // na
            rawJPChar.Add(0xE1, 'ニ'); // ni
            rawJPChar.Add(0xE2, 'ヌ'); // nu
            rawJPChar.Add(0xE3, 'ネ'); // ne
            rawJPChar.Add(0xE4, 'ノ'); // no

            rawJPChar.Add(0xE8, 'ハ'); // ha
            rawJPChar.Add(0xE9, 'ヒ'); // hi
            rawJPChar.Add(0xEA, 'フ'); // fu
            rawJPChar.Add(0xEB, 'ヘ'); // he
            rawJPChar.Add(0xEC, 'ホ'); // ho

            rawJPChar.Add(0x100, 'マ'); // ma
            rawJPChar.Add(0x101, 'ミ'); // mi
            rawJPChar.Add(0x102, 'ム'); // mu
            rawJPChar.Add(0x103, 'メ'); // me
            rawJPChar.Add(0x104, 'モ'); // mo

            rawJPChar.Add(0xA5, 'ヤ'); // ya
            rawJPChar.Add(0xA6, 'ユ'); // yu
            rawJPChar.Add(0xA7, 'ヨ'); // yo

            rawJPChar.Add(0x108, 'ラ'); // ra
            rawJPChar.Add(0x109, 'リ'); // ri
            rawJPChar.Add(0x10A, 'ル'); // ru
            rawJPChar.Add(0x10B, 'レ'); // re
            rawJPChar.Add(0x10C, 'ロ'); // ro

            rawJPChar.Add(0xAD, 'ワ'); // wa
            rawJPChar.Add(0xAE, 'ヲ'); // wo
            rawJPChar.Add(0xAF, 'ン'); // n

            //rawJPChar.Add(0x189, '－'); <- Exactly the same as hiragana; accounted for in the name-reading code

            rawJPChar.Add(0xC5, 'ガ'); // ga
            rawJPChar.Add(0xC6, 'ギ'); // gi
            rawJPChar.Add(0xC7, 'グ'); // gu
            rawJPChar.Add(0xCD, 'ゲ'); // ge
            rawJPChar.Add(0xCE, 'ゴ'); // go

            rawJPChar.Add(0xCF, 'ザ'); // za
            rawJPChar.Add(0xE5, 'ジ'); // ji
            rawJPChar.Add(0xE6, 'ズ'); // zu
            rawJPChar.Add(0xE7, 'ゼ'); // ze
            rawJPChar.Add(0xED, 'ゾ'); // zo

            rawJPChar.Add(0xEE, 'ダ'); // da
            rawJPChar.Add(0xEF, 'ヂ'); // di
            rawJPChar.Add(0x105, 'ヅ'); // du
            rawJPChar.Add(0x106, 'デ'); // de
            rawJPChar.Add(0x107, 'ド'); // do

            rawJPChar.Add(0x10D, 'バ'); // ba
            rawJPChar.Add(0x10E, 'ビ'); // bi
            rawJPChar.Add(0x10F, 'ブ'); // bu
            rawJPChar.Add(0x120, 'ベ'); // be
            rawJPChar.Add(0x121, 'ボ'); // bo

            rawJPChar.Add(0x122, 'パ'); // pa
            rawJPChar.Add(0x123, 'ピ'); // pi
            rawJPChar.Add(0x124, 'プ'); // pu
            rawJPChar.Add(0x125, 'ペ'); // pe
            rawJPChar.Add(0x126, 'ポ'); // po

            rawJPChar.Add(0x12B, 'ァ'); // ~a
            rawJPChar.Add(0x12C, 'ィ'); // ~i
            rawJPChar.Add(0x12D, 'ゥ'); // ~u
            rawJPChar.Add(0x12E, 'ェ'); // ~e
            rawJPChar.Add(0x12F, 'ォ'); // ~o

            rawJPChar.Add(0x127, 'ャ'); // ~ya
            rawJPChar.Add(0x128, 'ュ'); // ~yu
            rawJPChar.Add(0x129, 'ョ'); // ~yo

            rawJPChar.Add(0x12A, 'ッ'); // ~tsu

            // Romaji
            rawJPChar.Add(0x014A, 'A');
            rawJPChar.Add(0x014B, 'B');
            rawJPChar.Add(0x014C, 'C');
            rawJPChar.Add(0x014D, 'D');
            rawJPChar.Add(0x014E, 'E');
            rawJPChar.Add(0x014F, 'F');
            rawJPChar.Add(0x0160, 'G');
            rawJPChar.Add(0x0161, 'H');
            rawJPChar.Add(0x0162, 'I');
            rawJPChar.Add(0x0163, 'J');
            rawJPChar.Add(0x0164, 'K');
            rawJPChar.Add(0x0165, 'L');
            rawJPChar.Add(0x0166, 'M');
            rawJPChar.Add(0x0167, 'N');
            rawJPChar.Add(0x0168, 'O');
            rawJPChar.Add(0x0169, 'P');
            rawJPChar.Add(0x016A, 'Q');
            rawJPChar.Add(0x016B, 'R');
            rawJPChar.Add(0x016C, 'S');
            rawJPChar.Add(0x016D, 'T');
            rawJPChar.Add(0x016E, 'U');
            rawJPChar.Add(0x016F, 'V');
            rawJPChar.Add(0x0180, 'W');
            rawJPChar.Add(0x0181, 'X');
            rawJPChar.Add(0x0182, 'Y');
            rawJPChar.Add(0x0183, 'Z');
            //rawJPChar.Add(0x0189, '-'); <- Exactly the same as hiragana; accounted for in the name-reading code
            rawJPChar.Add(0x018E, '~');
        }
    }
}