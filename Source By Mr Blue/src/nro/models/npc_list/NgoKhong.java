package nro.models.npc_list;

import nro.models.consts.ConstNpc;
import nro.models.npc.Npc;
import nro.models.player.Player;
import nro.models.services.Service;

/**
 * NPC Ngộ Không - Ngũ Hành Sơn
 * @author NRO
 */
public class NgoKhong extends Npc {

    public NgoKhong(int mapId, int status, int cx, int cy, int tempId, int avatar) {
        super(mapId, status, cx, cy, tempId, avatar);
    }

    @Override
    public void openBaseMenu(Player player) {
        if (canOpenNpc(player)) {
            createOtherMenu(player, ConstNpc.BASE_MENU,
                "Tề Thiên Đại Thánh xin chào!\n"
                + "Ta đã bị phong ấn tại đây...\n"
                + "Hãy thu thập bùa để giải cứu ta!",
                "Hướng dẫn", "Đóng");
        }
    }

    @Override
    public void confirmMenu(Player player, int select) {
        if (canOpenNpc(player)) {
          if (player.idMark.isBaseMenu()) {
           if (select == 0) {
                     Service.gI().sendThongBao(player,
                        "Thu thập đủ 4 loại bùa:\n"
                        + "- Chữ GIẢI (10 cái)\n"
                        + "- Chữ KHAI (10 cái)\n"
                        + "- Chữ PHONG (10 cái)\n"
                        + "- Chữ ẤN (10 cái)\n"
                        + "Sau đó mang đến gặp Đường Tăng!");
                }
            }
        }
    }
}