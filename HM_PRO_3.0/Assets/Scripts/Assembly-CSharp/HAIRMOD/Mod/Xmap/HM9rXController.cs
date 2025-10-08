using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Logger;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ThreadAction;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Xmap
{
    internal class HM9rXController : ThreadActionUpdate<HM9rXController>, IActionListener
    {
        internal override int Interval => 100;

        static int mapEnd;
        static List<MapNext> way;
        static int indexWay;
        static bool isNextMapFailed;

        protected override void update()
        {
            ConsoleLogger.Log($"[xmap][dbg] update {mapEnd}", "yellow");
            if (way == null)
            {
                if (!isNextMapFailed)
                {
                    string mapName = TileMap.mapNames[mapEnd];
                    MainThreadDispatcher.Dispatch(() =>
                        GameScr.info1.addInfo(Strings.goTo + ": " + mapName, 0));
                }

                ConsoleLogger.Log($"[xmap][dbg] Đang tạo dữ liệu map", "yellow");
                XAlgorithm.xmapData = new XData();
                MainThreadDispatcher.Dispatch(XAlgorithm.xmapData.Load);
                while (!XAlgorithm.xmapData.isLoaded)
                    Thread.Sleep(100);
                XAlgorithm.xmapData.LoadLinkMapCapsule();
                try
                {
                    way = XAlgorithm.findWay(TileMap.mapID, mapEnd);
                    //if (way != null)
                    //LogMod.writeLine($"[xmap][dbg] bestWay: {JsonConvert.SerializeObject(way.Select(x => x.to).ToArray())}");
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Log($"[xmap][err] Lỗi tìm đường đi\n{ex}", "yellow");
                }
                indexWay = 0;

                if (way == null)
                {
                    MainThreadDispatcher.Dispatch(() =>
                        GameScr.info1.addInfo(Strings.xmapCantFindWay + '!', 0));
                    finishXmap();
                    return;
                }
            }

            if (TileMap.mapID == way[way.Count - 1].to && !Char.myCharz().IsCharDead())
            {
                MainThreadDispatcher.Dispatch(() =>
                    GameScr.info1.addInfo(Strings.xmapDestinationReached + '!', 0));
                finishXmap();
                return;
            }

            if (TileMap.mapID == way[indexWay].mapStart)
            {
                if (Char.myCharz().IsCharDead())
                {
                    Service.gI().returnTownFromDead();
                    isNextMapFailed = true;
                    way = null;
                }
                else if (Utils.CanNextMap())
                {
                    MainThreadDispatcher.Dispatch(() =>
                        HM9rX.NextMap(way[indexWay]));
                    ConsoleLogger.Log($"[xmap][dbg] nextMap: {way[indexWay].to}", "yellow");
                }
                Thread.Sleep(500);
                return;
            }
            else if (TileMap.mapID == way[indexWay].to)
            {
                indexWay++;
                return;
            }
            else
            {
                isNextMapFailed = true;
                way = null;
            }
        }
        internal static void start(int mapId)
        {
            if (gI.IsActing)
            {
                finishXmap();
                ConsoleLogger.Log($"[xmap][info] Hủy xmap tới {TileMap.mapNames[mapEnd]} để thực hiện xmap mới", "white");
            }
            mapEnd = mapId;
            gI.toggle(true);
            ConsoleLogger.Log($"[xmap][info] Bắt đầu xmap tới {TileMap.mapNames[mapEnd]}", "yellow");
        }

        internal static void finishXmap()
        {
            ConsoleLogger.Log($"[xmap][info] Kết thúc xmap", "white");
            way = null;
            isNextMapFailed = false;
            gI.toggle(false);
        }

        public void perform(int idAction, object p)
        {
            Action action = idAction switch
            {
                1 => () =>
                {
                    MyVector myVector = new MyVector();
                    List<int> mapID = (List<int>)p;
                    for (int i = 0; i < mapID.Count; i++)
                    {
                        string str = HM9rX.getMapName(mapID[i]);
                        myVector.addElement(new Command(str, gI, 3, mapID[i]));
                    }
                    GameCanvas.menu.startAt(myVector, 0);
                },
                2 => () => HM9rX.ShowXmapSettings(),
                3 => () =>
                {
                    int mapID = (int)p;
                    start(mapID);
                },
                4 => () => HM9rX.ToggleUseCapsuleNormal(),
                5 => () => HM9rX.ToggleUseCapsuleVip(),
                6 => () =>
                {
                    Action action1 = () => GameScr.info1.addInfo(Utils.ToggleStringRefByBoolean(Strings.xmapUseAStar, HM9rX.isXmapAStar), 0);
                    Utils.SetToggleRefByBoolean(action1, ref HM9rX.isXmapAStar);
                },
                _ => () => { }
            };
            action();
        }
    }
}
