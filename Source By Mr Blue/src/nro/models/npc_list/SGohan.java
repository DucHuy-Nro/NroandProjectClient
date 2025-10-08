package nro.models.npc_list;

import nro.models.consts.ConstNpc;
import nro.models.npc.Npc;
import nro.models.player.Player;
import nro.models.services.Service;
import nro.models.shop.ShopService;

public class SGohan extends Npc {

    public SGohan(int mapId, int status, int cx, int cy, int tempId, int avartar) {
        super(mapId, status, cx, cy, tempId, avartar);
    }
    @Override
    public void openBaseMenu(Player player) {
        if (canOpenNpc(player)) {
            createOtherMenu(player, ConstNpc.BASE_MENU,
                "Xin chào, tôi bán tuyệt kĩ!","Cửa hàng");
        }
    }

    @Override
    public void confirmMenu(Player player, int select) {
        if (canOpenNpc(player)) {
            switch (select) {
                case 0: // Mua tuyệt kỹ
                   ShopService.gI().opendShop(player, "SHOP_TUYET_KY", true);
                    
                    break;
            }
        }
    }
}