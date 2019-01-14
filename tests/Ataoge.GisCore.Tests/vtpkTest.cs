using System;
using System.IO;
using System.IO.Compression;
using Ataoge.GisCore.Wmts;
using Xunit;

namespace Ataoge.GisCore.Tests
{
    public class vtpkTest
    {
        [Fact]
        public void TestVtpk()
        {
            var path = @"E:\Downloads\wh_district_slqp_WebUTM.vtpk";
            var zipArchive = ZipFile.Open(path, ZipArchiveMode.Read);
            var entry = zipArchive.GetEntry(@"p12/tile/L00/R0000C0000.bundle");
            var stream = entry.Open();
            if (!stream.CanSeek)
            {
                //for(var i=0; i<64; i++)
                //    stream.ReadByte();
            }
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms, (int)entry.CompressedLength);

            
            ms.Seek(4, SeekOrigin.Begin);
             byte[] fileSizeBytes = new byte[4];
                ms.Read(fileSizeBytes, 0, 4);
                int fileSize = BitConverter.ToInt32(fileSizeBytes, 0);
            
            ms.Seek(64, SeekOrigin.Begin);
            for(var i =0; i < 128; i++)
            {
                //获取位置索引并计算切片位置偏移量
                byte[] indexBytes = new byte[8];
                ms.Read(indexBytes, 0, 8); 
                var  offset = BitConverter.ToInt64(indexBytes, 0);
                var ss  = offset >> 40;
                byte[] sizeBytes = new byte[4];
                ms.Read(sizeBytes, 0, 3);
                Console.WriteLine($"{offset}");  
               
           var size =BitConverter.ToInt32(sizeBytes, 0);
                             

                //获取切片长度索引并计算切片长度
                long startOffset = offset - 4; 
                ms.Seek(startOffset, SeekOrigin.Begin);
                byte[] lengthBytes = new byte[4];
                ms.Read(lengthBytes, 0, 4);
                int length = BitConverter.ToInt32(lengthBytes, 0);
                 byte[] tileBytes = new byte[length];
                int bytesRead = ms.Read(tileBytes, 0, tileBytes.Length);
            }
        }

        [Fact]
        public void TestVtpkJson()
        {
            var path = @"E:\Downloads\wh_district_slqp_WebUTM.vtpk";
            var zipArchive = ZipFile.Open(path, ZipArchiveMode.Read);
            var entry = zipArchive.GetEntry(@"p12/root.json");
            var stream = entry.Open();
            var sr = new StreamReader(stream);
            var ts = ArcGISBundleFileHelper.GetTileSystemFromJson( sr.ReadToEnd());
            sr.Close();
            stream.Close();
        }

        [Fact]
        public void TestTile()
        {
            //var aa = ArcGISBundleFileHelper.GetTileImage_VTPK(8,104,209, @"E:\Downloads\wh_district_slqp_WebUTM.vtpk");
            ArcGISTileSystem ts = new ArcGISTileSystem();
            ts.SetXYOriginShift(20037508.342787001, -20037508.342787001);
            ts.SetInitialResolution(78271.516964011724);
            ts.SetExtent(12655752.565389978,3499586.0153526235,12810509.139444513, 3679682.1540471311);
            var xx = ts.XYToTile(12655752.565389978, 3499586.0153526235, 8);
            var tt = ts.XYBoundsToTileMatrix(12655752.565389978,3499586.0153526235,12810509.139444513, 3679682.1540471311, 9);
            ArcGISTileSystem wh = new ArcGISTileSystem();
            wh.SetTileSize(512);
            wh.SetXYOrigin(-9201964.7292280346, 7001964.7292280346);
            wh.SetInitialResolution(39070.17472354701);
            for(var i =0; i < 20; i++)
            {
                var tm = wh.XYBoundsToTileMatrix(738652.7145999996,316921.70600000024, 870924.74730000016, 470966.16030000057,i);
                Console.WriteLine($"{i} {tm.MinRow} {tm.MaxRow}  {tm.MinCol} {tm.MaxRow}");
            }

        }
    }
}