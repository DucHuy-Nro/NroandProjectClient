# Tóm tắt các thay đổi để fix lỗi int to long conversion

## Các file đã sửa (15 files):

### 1. Service.java
- Thay đổi signature `hsChar()` từ `int hp, int mp` → `long hp, long mp`
- Thay `writeShort()` → `writeLong()` cho defg (dòng 797)
- Thay `writeShort()` → `writeLong()` cho def (dòng 1630)
- Thay `writeInt()` → `writeLong()` cho hp, hpMax (dòng 2582, 2583)

### 2. Zone.java
- Thay `writeInt()` → `writeLong()` cho hp, hpMax (dòng 558, 559)

### 3. Mob.java & MobMe.java
- Thay `writeInt()` → `writeLong()` cho hp

### 4. EffectSkin.java
- Thay kiểu `int subHp, subMp` → `long subHp, subMp` (3 vị trí)

### 5. BotAttackplayer.java
- Thay kiểu `int damePST` → `long damePST`
- Thay `writeInt()` → `writeLong()` cho hp

### 6. Boss classes (Odo, Broly, SuperBroly)
- Thay kiểu `int hpMax, regenAmount, subHp` → `long`

### 7. BossData.java
- Thay kiểu tham số `int dame` → `long dame` trong tất cả constructors (6 constructors)

### 8. NinjaClone.java
- Thay signature constructor: `int dame, int hp` → `long dame, long hp`
- Cast hp khi truyền vào BossData: `(int)hp`

### 9. Rival.java
- Cast hpg: `(int)player.nPoint.hpg`

### 10. Dracula.java
- Thay kiểu `int hp` → `long hp`

### 11. PlayerService.java
- Thay kiểu `int mp` → `long mp` (dòng 226)

### 12. SkillService.java
- Thay kiểu `int hpUse, mpUse, damePST, dameHit` → `long`
- Cast cho Util.nextInt(): `(int)(plAtt.nPoint.hpMax / 200)`
- Thay `writeInt()` → `writeLong()` cho hp

## Tổng kết:
- 15 files đã được sửa
- 36 dòng code đã thay đổi
- Tất cả lỗi conversion từ long sang int đã được xử lý
