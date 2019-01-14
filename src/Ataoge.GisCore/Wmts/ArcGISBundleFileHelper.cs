using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Ataoge.GisCore.Wmts
{
    public static class ArcGISBundleFileHelper
    {
        /// <summary>
        /// 切图原点在左下角的切图原点在左上角的切片互转
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="zoom"></param>
        /// <param name="otx"></param>
        /// <param name="oty"></param>
        public static void TileXYToTileXY(int tx, int ty, int zoom, out int otx, out int oty)
        {
            otx = tx;
            oty = (1 << zoom) - 1 - ty;
        }

        /// <summary>
        /// ArcGIS兼容的Tile转微软Bing地图的QuadKey
        /// 即切图原点在左上角
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static string TileXYToQuadKey(int tileX, int tileY, int zoom)
        {
            StringBuilder quadKey = new StringBuilder();
            for (int i = zoom; i > 0; i--)
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }
                if ((tileY & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }

        /// <summary>
        /// Converts a QuadKey into tile XY coordinates
        /// </summary>
        /// <param name="quadKey"></param>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <param name="zoom"></param>
        public static void QuadKeyToTileXY(string quadKey, out int tileX, out int tileY, out int zoom)
        {
            tileX = tileY = 0;
            zoom = quadKey.Length;
            for (int i = zoom; i > 0; i--)
            {
                int mask = 1 << (i - 1);
                switch (quadKey[zoom - i])
                {
                    case '0':
                        break;

                    case '1':
                        tileX |= mask;
                        break;

                    case '2':
                        tileY |= mask;
                        break;

                    case '3':
                        tileX |= mask;
                        tileY |= mask;
                        break;

                    default:
                        throw new ArgumentException("Invalid QuadKey digit sequence.");
                }
            }
        }

        public static string GetBundleFilePath(int zoom, int tx, int ty, string basePath = "_alllayers")
        {
            int packetSize = 128;
            int startCol = (ty / packetSize) * packetSize;
            int startRow = (tx / packetSize) * packetSize;
            //string basePath = "_alllayers";
            string layerPath = string.Format("L{0:D2}", zoom);
            string fileName = string.Format("R{0:x4}C{1:x4}", startRow, startCol);
            return Path.Combine(basePath, layerPath, fileName + ".bundle");
        }

        public static byte[] GetTileImage_v2(int zoom, int tx, int ty, string basePath = "_alllayers")
        {
            int packetSize = 128;
            int startCol = (ty / packetSize) * packetSize;
            int startRow = (tx / packetSize) * packetSize;
            string layerPath = string.Format("L{0:D2}", zoom);
            string fileName = string.Format("R{0:x4}C{1:x4}", startRow, startCol);
            string bundleFileName = Path.Combine(basePath, layerPath, fileName + ".bundle");
            if (string.IsNullOrEmpty(bundleFileName) || !File.Exists(bundleFileName))
            {
                return null;
            }
            
            int index = packetSize * (tx - startRow) + (ty - startCol);

            FileStream fs = new FileStream(bundleFileName, FileMode.Open, FileAccess.Read);
            fs.Seek(64 + 8 * index, SeekOrigin.Begin);
                        
            //获取位置索引并计算切片位置偏移量
            byte[] indexBytes = new byte[8];
            fs.Read(indexBytes, 0, 8); 
            var indexOffset = BitConverter.ToInt64(indexBytes, 0);  
            var offset = (indexOffset << 24) >> 24;
            var size = indexOffset >> 40;
            if (size == 0)
                return null; 
          
            //获取切片长度索引并计算切片长度
            long startOffset = offset - 4; 
            fs.Seek(startOffset, SeekOrigin.Begin);
            byte[] lengthBytes = new byte[4];
            fs.Read(lengthBytes, 0, 4);
            int length = BitConverter.ToInt32(indexBytes, 0);

            //根据切片位置和切片长度获取切片
            byte[] tileBytes = new byte[length];
            int bytesRead = fs.Read(tileBytes, 0, tileBytes.Length);
            if(bytesRead > 0){
                return tileBytes;
            }
            return null;
            /* 
            MemoryStream ms = new MemoryStream();
            
            byte[] tileBytes = new byte[length];
            int bytesRead = 0;
            if(length > 0){
                bytesRead = fs.Read(tileBytes, 0, tileBytes.Length);
                if(bytesRead > 0){
                    ms.Write(tileBytes, 0, bytesRead);
                }
            }
            return ms.ToArray();
            */
        }

        public static void InitBundleFile(string filePath)
        {
            int version = 3;
            int recordCount = 16384; // 128*128
            int maximumTileSize = 0;
            int offsetByteCount = 5;
            long slackSpace = 0;
            long fileSize = 64 + 131072;
            long userHeaderOffset = 40;
            int userHeaserSize = 20 + 131072;
            int  legacyVesion = 3;
            int legacySize = 16;
            int legacyRecourdCount = 16384;
            int legacyOffsetByteCount = 5;
            int indexSize = 131072;

            if (File.Exists(filePath))
                return;

            FileStream fs = File.OpenWrite(filePath);
            fs.Write(BitConverter.GetBytes(version), 0, 4);
            fs.Write(BitConverter.GetBytes(recordCount), 0, 4);
            fs.Write(BitConverter.GetBytes(maximumTileSize), 0, 4);
            fs.Write(BitConverter.GetBytes(offsetByteCount), 0, 4);
            fs.Write(BitConverter.GetBytes(slackSpace), 0, 8);
            fs.Write(BitConverter.GetBytes(fileSize), 0, 8);
            fs.Write(BitConverter.GetBytes(userHeaderOffset), 0, 8);
            fs.Write(BitConverter.GetBytes(userHeaserSize), 0, 4);
            fs.Write(BitConverter.GetBytes(legacyVesion), 0, 4);
            fs.Write(BitConverter.GetBytes(legacySize), 0, 4);
            fs.Write(BitConverter.GetBytes(legacyRecourdCount), 0, 4);
            fs.Write(BitConverter.GetBytes(legacyOffsetByteCount), 0, 4);
            fs.Write(BitConverter.GetBytes(indexSize), 0, 4);

            byte[] bytes = new byte[indexSize];
            fs.Write(bytes, 0, indexSize);
            fs.Close();

        }

        public static void CleanUpBundleFile(string filePath)
        {
            FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            var fileSize = fs.Length;
            fs.Seek(24, SeekOrigin.Begin);
            fs.Write(BitConverter.GetBytes(fileSize), 0, 8);
            fs.Close();
        }

        public static byte[] GetTileImage_VTPK(int zoom, int tx, int ty, string filePath)
        {
            int packetSize = 128;
            int startCol = (ty / packetSize) * packetSize;
            int startRow = (tx / packetSize) * packetSize;
            string layerPath = string.Format("L{0:D2}", zoom);
            string fileName = string.Format("R{0:x4}C{1:x4}", startRow, startCol);

            var zipArchive = ZipFile.Open(filePath, ZipArchiveMode.Read);
            var entry = zipArchive.GetEntry($"p12/tile/{layerPath}/{fileName}.bundle");
            var stream = entry.Open();
            var tileIndexOffset = 64 + 8 * (packetSize * (tx % packetSize) + (ty % packetSize));
            if (stream.CanSeek)
            {
                stream.Seek(tileIndexOffset, SeekOrigin.Begin);
            }
            else
            {
                for(var i=0; i<tileIndexOffset; i++)
                    stream.ReadByte();
            }

            byte[] indexBytes = new byte[8];
            stream.Read(indexBytes, 0, 8); 
            var  index = BitConverter.ToInt64(indexBytes, 0);
            var offset = (index << 24) >> 24;
            var size = index >> 40;
            if (size == 0)
                return null;
            
            if (stream.CanSeek)
            {
                stream.Seek(offset, SeekOrigin.Begin);   
            }
            else
            {
                offset = offset - tileIndexOffset - 8;
                 for(var i=0; i<offset; i++)
                    stream.ReadByte();
            }

            byte[] tileBytes = new byte[size];
            int bytesRead = stream.Read(tileBytes, 0, tileBytes.Length);
            if (bytesRead == size)
                return tileBytes;
            return null;
        }

        public static byte[] GetTileImage_v1(int zoom, int tx, int ty, string basePath = "_alllayers")
        {
            int packetSize = 128;
            int startCol = (ty / packetSize) * packetSize;
            int startRow = (tx / packetSize) * packetSize;
            //string basePath = "_alllayers";
            string layerPath = string.Format("L{0:D2}", zoom);
            string fileName = string.Format("R{0:x4}C{1:x4}", startRow, startCol);
            string bundlxFileName = Path.Combine(basePath, layerPath, fileName + ".bundlx");
            string bundleFileName = Path.Combine(basePath, layerPath, fileName + ".bundle");
            if ((string.IsNullOrEmpty(bundlxFileName) || !File.Exists(bundlxFileName)) || (string.IsNullOrEmpty(bundleFileName) || !File.Exists(bundleFileName)))
            {
                return null;
            }
            try
            {
                int col = ty - startCol;
                int row = tx - startRow;
                int offsetLength = 0;
                using (FileStream fs = new FileStream(bundlxFileName, FileMode.Open, FileAccess.Read))
                {
                    int bdxPos = 16 + (col * packetSize + row) * 5;
                    fs.Seek(bdxPos, SeekOrigin.Begin);
                    byte[] buffer = new byte[5];
                    fs.Read(buffer, 0, 5);
                    offsetLength = BitConverter.ToInt32(buffer, 0);
                    fs.Close();
                }
                //偏移量指向空数据
                if (offsetLength == 0 || offsetLength == (60 + (col * packetSize + row) * 4))
                {
                    return null;
                }
                byte[] imageData = null;
                using (FileStream fs = new FileStream(bundleFileName, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(offsetLength, SeekOrigin.Begin);
                    byte[] buffer = new byte[4];
                    fs.Read(buffer, 0, 4);
                    int imageLength = BitConverter.ToInt32(buffer, 0);
                    imageData = new byte[imageLength];
                    fs.Read(imageData, 0, imageLength);
                    fs.Close();
                }
                return imageData;
            }
            catch (Exception)
            {
                
            }
            return null;
        }

         public static void ReadArcGISBundleFile(string basePath, string bundleName)
        {
            int packetSize = 128;
            int cindex = bundleName.IndexOf("C");
            int startRow = Int32.Parse(bundleName.Substring(1, cindex - 1), System.Globalization.NumberStyles.HexNumber);
            int startCol = Int32.Parse(bundleName.Substring(cindex + 1), System.Globalization.NumberStyles.HexNumber);

            string bundlxFileName = Path.Combine(basePath, bundleName + ".bundlx");
            using (FileStream fs = new FileStream(bundlxFileName, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(16, SeekOrigin.Begin);
                int noData = 0;
                int hasData = 0;
                int col = startCol;
                for (int j = 0; j < packetSize; j++)
                {
                    int row = startRow;
                    for (int i = 0; i < packetSize; i++)
                    {
                        //int bdxPos = 16 + (j * packetSize + i) * 5;
                        //fs.Seek((long)bdxPos, SeekOrigin.Begin);
                        byte[] buffer = new byte[5];
                        fs.Read(buffer, 0, 5);
                        int offset = (int)GetLongFromBytes(buffer, true);
                        if (offset == (60 + (j * packetSize + i) * 4))
                        {
                            noData++;
                            Console.WriteLine(string.Format("行{0}列{1}{2}影像", row, col, "无"));
                        }
                        else
                        {
                            hasData++;
                            Console.WriteLine(string.Format("行{0}列{1}{2}影像", row, col, "有"));
                        }
                        row++;
                    }
                    col++;
                }
                Console.WriteLine(string.Format("共{0}有数据，{1}无数据", hasData, noData));
            }
        }

        public static long GetLongFromBytes(byte[] buf, bool asc)
        {
            if (buf == null)
            {
                throw new Exception("byte array is null!");
            }
            if (buf.Length > 8)
            {
                throw new Exception("byte array size > 8 !");
            }
            ulong num = 0;
            if (asc)
            {
                for (int j = buf.Length - 1; j >= 0; j--)
                {
                    num = num << 8;
                    num |= ((ulong)((ulong)buf[j] & 0xff));
                }
                return (long)num;
            }
            for (int i = 0; i < buf.Length; i++)
            {
                num = num << 8;
                num |= (ulong)((ulong)buf[i] & 0xff);
            }
            return (long)num;
        }


        public static void CreateArcGISBundleFile(string basePath, string bundleName, int zoom, int minTX, int minTY, int maxTX, int maxTY)
        {
            byte[] bdxBts = new byte[16] { 0x03, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00 };

            byte[] bdlxEndBts = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] bdleBts = new byte[44] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, //8-11表示第一个非空，非全透明图片的大小
                                      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //16-19位，表示非空文件个数*4  24-27位 文件大小
                                      0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00 };


            string bundlePath = string.Format("L{0:D2}", zoom);
            string path = System.IO.Path.Combine(basePath, bundlePath);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }


            int packetSize = 128;
            int cindex = bundleName.IndexOf("C");
            int row = Int32.Parse(bundleName.Substring(1, cindex - 1), System.Globalization.NumberStyles.HexNumber);
            int col = Int32.Parse(bundleName.Substring(cindex + 1), System.Globalization.NumberStyles.HexNumber);

            byte[] bundlx = new byte[16 + 128 * 128 * 5 + 16]; //81952;
            Array.Copy(bdxBts, 0, bundlx, 0, 16);
            Array.Copy(bdlxEndBts, 0, bundlx, 81952 - 16 - 1, 16);

            int cPos = 0;
            MemoryStream ms = new MemoryStream();
            //BufferedStream bufStream = new BufferedStream(memStream);
            //写文件头
            ms.Write(bdleBts, 0, 44);
            cPos += 44;
            //写开始行
            ms.Write(BitConverter.GetBytes(row), 0, 4);
            cPos += 4;
            //写开始列
            ms.Write(BitConverter.GetBytes(col), 0, 4);
            cPos += 4;
            //写结束行
            ms.Write(BitConverter.GetBytes(row + packetSize - 1), 0, 4);
            cPos += 4;
            //写结束列
            ms.Write(BitConverter.GetBytes(col + packetSize - 1), 0, 4);
            cPos += 4;

            //写入空值指针
            int nullOffset = cPos;
            int nullValueSize = packetSize * packetSize * 4;
            byte[] nullValue = new byte[nullValueSize];
            ms.Write(nullValue, 0, nullValueSize);
            cPos += nullValueSize;

            int valueCount = 0;
            int ty = col;
            int firstNotNullImageSize = 0;
            for (int j = 0; j < packetSize; j++) //按列
            {
                int tx = row;
                for (int i = 0; i < packetSize; i++) //按行
                {
                    int bdxPos = 16 + (j * packetSize + i) * 5;
                    int nullValuePos = nullOffset + (j * packetSize + i) * 4;
                    //判断是否在范围内
                    if ((i >= minTX && i <= maxTX) && (j >= minTY && j <= maxTY))
                    {
                        //获取 zoom, tx, ty的图像
                        byte[] imageBytes = GetTileImage(tx, ty, zoom);
                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            //取第一个非空透明图像的大小
                            if (firstNotNullImageSize == 0)
                                firstNotNullImageSize = imageBytes.Length;

                            Array.Copy(BitConverter.GetBytes(cPos), 0, bundlx, bdxPos, 4);

                            ms.Write(BitConverter.GetBytes(imageBytes.Length), 0, 4);
                            cPos += 4;
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            cPos += imageBytes.Length;
                            nullValuePos = 0;
                            valueCount++;
                        }
                    }

                    if (nullValuePos > 0)
                    {
                        Array.Copy(BitConverter.GetBytes(nullValuePos), 0, bundlx, bdxPos, 4);
                    }
                    tx++;
                }
                ty++;
            }

            //bundle文件总长
            int bundleLength = (int)ms.Length;
            byte[] bundle = ms.ToArray();
            ms.Close();
            //ms.Position = 0;
            //ms.Write(BitConverter.GetBytes(firstNotNullImageSize), 8, 4);
            //ms.Write(BitConverter.GetBytes(valueCount * 4), 4, 4);
            //ms.Write(BitConverter.GetBytes(bundleLength), 4, 4);
            Array.Copy(BitConverter.GetBytes(firstNotNullImageSize), 0, bundle, 8, 4);
            Array.Copy(BitConverter.GetBytes(valueCount * 4), 0, bundle, 16, 4);
            Array.Copy(BitConverter.GetBytes(bundleLength), 0, bundle, 24, 4);

            FileStream fs = File.Open(Path.Combine(path, bundleName + ".bundlx"), FileMode.Create);
            fs.Write(bundlx, 0, bundlx.Length);
            fs.Close();

            FileStream fs2 = File.Open(Path.Combine(path, bundleName + ".bundle"), FileMode.Create);
            fs2.Write(bundle, 0, bundleLength);
            fs2.Close();
        }

        public static byte[] GetTileImage(int tx, int ty, int zoom)
        {
            return new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x00, 0x00, 0x00, 0x0d, 0x49, 0x49 };
        }

        public static void ToFile(byte[] bytes, string fileName)
        {
            byte[] jpgHeader = new byte[2] { 0xff, 0xd8 }; //文件结束标识 0xff, 0xd9 
            byte[] pngHeader = new byte[8] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
            byte[] tgaHeader = new byte[5] { 0x00, 0x00, 0x20, 0x00, 0x00 }; //RLE压缩  { 0x00, 0x00, 0x10, 0x00, 0x00 }
            byte[] gifHeader = new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };// { 0x47, 0x49, 0x46, 0x38, 0x39, (0x37), 0x61 }
            byte[] bmpHeader = new byte[2] { 0x42, 0x4d };
            byte[] pcxHeader = new byte[1] { 0x0a };
            byte[] tiffHeader = new byte[2] { 0x4d, 0x4d };//或者 {0x49, 0x49}
            byte[] icoHeader = new byte[8] { 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x20, 0x20 };
            byte[] curHeader = new byte[8] { 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x20, 0x20 };
            byte[] iffHeader = new byte[4] { 0x46, 0x4f, 0x52, 0x4d };
            byte[] aniHeader = new byte[4] { 0x52, 0x49, 0x46, 0x46 };
            using (FileStream fs = File.Open(fileName, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

            //CAD(dwg)，文件头：41433130  
            //Adobe Photoshop (psd)，文件头：38425053 Rich Text Format(rtf)，文件头：7B5C727466 XML(xml)，文件头：3C3F786D6C HTML(html)，文件头：68746D6C3E  Email[thorough only](eml)，文件头：44656C69766572792D646174653A Outlook Express(dbx)，文件头：CFAD12FEC5FD746F Outlook(pst)，文件头：2142444E  MS Word / Excel(xls.or.doc)，文件头：D0CF11E0 MS Access(mdb)，文件头：5374616E64617264204A WordPerfect(wpd)，文件头：FF575043  Postscript(eps.or.ps)，文件头：252150532D41646F6265 Adobe Acrobat(pdf)，文件头：255044462D312E Quicken(qdf)，文件头：AC9EBD8F  Windows Password(pwl)，文件头：E3828596 ZIP Archive(zip)，文件头：504B0304 RAR Archive(rar)，文件头：52617221
            //Wave(wav)，文件头：57415645 AVI(avi)，文件头：41564920 Real Audio(ram)，文件头：2E7261FD Real Media(rm)，文件头：2E524D46 MPEG(mpg)，文件头：000001BA MPEG(mpg)，文件头：000001B3 Quicktime(mov)，文件头：6D6F6F76  Windows Media(asf)，文件头：3026B2758E66CF11 MIDI(mid)，文件头：4D546864
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static ArcGISTileSystem GetTileSystemFromXmlConf(string xmlPath)
        {
            XDocument xmlDoc =  XDocument.Load(xmlPath);
            var elmTileOrigin = xmlDoc.Element("CacheInfo").Element("TileCacheInfo").Element("TileOrigin");
            var originX = double.Parse(elmTileOrigin.Element("X").Value);
            var originY = double.Parse(elmTileOrigin.Element("Y").Value);
            var elmTileSize = xmlDoc.Element("CacheInfo").Element("TileCacheInfo").Element("TileCols");
            var tileSize = int.Parse(elmTileSize.Value);
            var elmLodInfos = xmlDoc.Element("CacheInfo").Element("TileCacheInfo").Element("LODInfos");
            var zeroScale = 0.0;
            var zeroResolution = 0.0;
            foreach(var elm in elmLodInfos.Elements("LODInfo"))
            {
                if (int.TryParse(elm.Element("LevelID").Value, out int level))
                {
                    if (level == 0)
                    {
                        zeroScale = double.Parse(elm.Element("Scale").Value);
                        zeroResolution = double.Parse(elm.Element("Resolution").Value);
                        break;
                    }
                }
            }

            var cdiPath = Path.ChangeExtension(xmlPath, ".cdi");
            xmlDoc =  XDocument.Load(cdiPath);
            var elmFullExtent = xmlDoc.Element("EnvelopeN");
            var xMin = double.Parse(elmFullExtent.Element("XMin").Value);
            var yMin = double.Parse(elmFullExtent.Element("YMin").Value);
            var xMax = double.Parse(elmFullExtent.Element("XMax").Value);
            var yMax = double.Parse(elmFullExtent.Element("YMax").Value);

            ArcGISTileSystem tileSystem = new ArcGISTileSystem();
            tileSystem.SetTileSize(tileSize);
            tileSystem.SetXYOrigin(originX, originY);
            tileSystem.SetInitialResolution(zeroResolution);
            tileSystem.SetInitailScale(zeroScale);
            tileSystem.SetExtent(xMin, yMin, xMax, yMax);
            return tileSystem;
        }

        public static ArcGISTileSystem GetTileSystemFromJson(string jsonString)
        {
            var root = JObject.Parse(jsonString);
            var fullExtent = root["fullExtent"];
            var xMin = fullExtent["xmin"].Value<double>();
            var yMin = fullExtent["ymin"].Value<double>();
            var xMax = fullExtent["xmax"].Value<double>();
            var yMax = fullExtent["ymax"].Value<double>();
            var tileInfo = root["tileInfo"];
            var tileSize = tileInfo["rows"].Value<int>();
            var originX = tileInfo["origin"]["x"].Value<double>();
            var originY = tileInfo["origin"]["x"].Value<double>();

            var zeroScale = 0.0;
            var zeroResolution = 0.0;
            var lods = tileInfo["lods"] as JArray;
            foreach(var lod in lods)
            {
                int level = lod["level"].Value<int>();
                if (level == 0)
                {
                    zeroScale = lod["scale"].Value<double>();
                    zeroResolution = lod["resolution"].Value<double>();
                    break;
                }
            }
            ArcGISTileSystem tileSystem = new ArcGISTileSystem();
            tileSystem.SetTileSize(tileSize);
            tileSystem.SetXYOrigin(originX, originY);
            tileSystem.SetInitialResolution(zeroResolution);
            tileSystem.SetInitailScale(zeroScale);
            tileSystem.SetExtent(xMin, yMin, xMax, yMax);
            return tileSystem;
        }
        
    }
}