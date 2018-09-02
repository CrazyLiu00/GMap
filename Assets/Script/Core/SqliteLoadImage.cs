using UnityEngine;
using System.Collections;
using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.IO;
using Mono.Data.Sqlite;
using Alt.Sketch.GMap.NET;
using System.Data;


public class SqliteLoadImage : PureImageCache{

    string connectstring = string.Empty;
    public static string DataPath = "F:/OutSource/DataExp.gmdb";
    public string ConnectString
    {
        get {
            return connectstring;
        }
        set
        {
            if (connectstring != value)
            {
                connectstring = value;

                //if (Initialized)
                //{
                //    Dispose();
                //    Initialize();
                //}
            }
        }

    }

    //private SqliteDataReader sq_reader;
    //private SqliteConnection cn;
    PureImage PureImageCache.GetImageFromCache(int type, GPoint pos, int zoom)
    {
        PureImage ret = null;
        //AltSketchImage img = null;
        try {
            
            using (SqliteConnection cn = new SqliteConnection())
            {

                //cn.ConnectionString = "Data Source=\"E:\\Work_Project\\TwoMap\\TwoMap_testMap\\TwoMap_test_02\\Assets\\StreamingAssets\\Data_new.gmdb\";Page Size=32768;Pooling=True";                
               // cn.ConnectionString = "Data Source=\"E:\\Work_Project\\TwoMap\\Formal_Map\\ErSituation\\Assets\\StreamingAssets\\Data.gmdb\";Page Size=32768;Pooling=True";
             ///   cn.ConnectionString = "Data Source=\"E:\\NewSituation\\NewSituation_Data\\StreamingAssets\\Data.gmdb\";Page Size=32768;Pooling=True";
                cn.ConnectionString = "Data Source="+DataPath+";Page Size=32768;Pooling=True"; 
                object mylock = "lock111";
                lock (mylock)
                {
                    //Debug.Log("---");
                    cn.Open();
                    {
                        using (SqliteCommand sq_com = cn.CreateCommand())
                        {

                            sq_com.CommandText = "select Tile from TilesData where id=(select id from Tiles where X=" + pos.X + " AND Y=" + pos.Y + " AND Zoom=" + zoom + " AND Type=" + type + ")";//+" AND Type="+type 
                        
                            using (SqliteDataReader sq_reader = sq_com.ExecuteReader(System.Data.CommandBehavior.SequentialAccess))
                            {
                                if (sq_reader.Read())
                                {
                                    //Byte[] tile = (Byte[])sq_reader["Tile"];
                                    long len = sq_reader.GetBytes(0, 0, null, 0, 0);
                                    //Debug.Log("len = " + len);
                                    byte[] tile = new byte[len];                                    
                                    //Stream stream = null;                               
                                    sq_reader.GetBytes(0, 0, tile, 0, tile.Length);
                                    {                                        

#if Stream
                                        #region  换成数据流方式加载
                                    MemoryStream stream = new MemoryStream(tile,0,tile.Length,false,true);
                                    if (GMapProvider.TileImageProxy != null && stream.Length > 0)
                                    {
                                        //ret = AltSketchImageProxy.Instance.FromStream(stream);

                                        //stream.Position = 0;
                                        ret = GMapProvider.TileImageProxy.FromStream(stream);
                                        if (ret != null)
                                        {

                                            ret.Data = stream;
                                            ret.Data.Position = 0;
                                            Debug.Log("X = " + pos.X + "   Y = " + pos.Y + " Z= " + zoom + "  title = " + tile.Length);
                                        }
                                        else
                                        {
                                            stream.Dispose();
                                        }
                                    }
                                    stream = null;
                                        #endregion
#else
                                        #region 直接加载
                                        if (tile != null && tile.Length > 0)
                                        {                                           
                                            if (GMapProvider.TileImageProxy != null)
                                            {
                                                ret = GMapProvider.TileImageProxy.FromArray(tile);
                                            }
                                            //Debug.Log("x = " + pos.X + "   y = " + pos.Y + "  zoom = " + zoom + " data = " + ret.Data.Length);
                                        }
                                        #endregion
#endif
                                    }
                                    tile = null;
                                }
                                sq_reader.Close();
                            }
                        }
                    }
                    cn.Close();
                }
            }

        }
        catch(Exception ex)
        {
            Debug.Log("GetImageFromCache " + ex.ToString());
            ret = null;
        }

        //Debug.Log(zoom);
        //Debug.Log("ret = " + ret.Data.Length);
        //long m_time = 100000000;
        //while (m_time > 0)
        //{
        //    m_time--;
        //}

        
        return ret;
    }

    int PureImageCache.DeleteOlderThan(DateTime date, int? type)
    {
        Debug.Log("DeleteOlderThan");
        return 0;
    }

    bool PureImageCache.PutImageToCache(byte[] tile, int type, GPoint pos, int zoom)
    {
        Debug.Log("PutImageToCache");
        return true;
    }

	
}
