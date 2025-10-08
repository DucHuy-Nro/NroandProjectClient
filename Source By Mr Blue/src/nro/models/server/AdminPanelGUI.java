package nro.models.server;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import nro.models.utils.Logger;

/**
 * GUI Admin Panel - Cửa sổ điều khiển server
 * @author NRO Admin Panel
 */
public class AdminPanelGUI extends JFrame {

    private JLabel lblServerName;
    private JLabel lblStatus;
    private JLabel lblPlayersOnline;
    private JLabel lblExpRate;
    private JLabel lblUptime;
private JLabel lblCpu;
private JLabel lblRam;
private JLabel lblThreads;
    private JButton btnMaintenance20s;
    private JButton btnKickAll;
    private JButton btnMaintenanceNow;
    private JButton btnRefresh;
    private JButton btnApplyExp;

    private JComboBox<String> cboExpRate;

    private Timer updateTimer;

    public AdminPanelGUI() {
        initComponents();
        startAutoUpdate();
    }

    private void initComponents() {
        // Cấu hình JFrame
        setTitle("🔧 NRO SERVER - ADMIN PANEL 🔧");
        setSize(550, 580);
        setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
        setLocationRelativeTo(null);
        setResizable(false);

        // Xử lý sự kiện đóng cửa sổ
        addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                int confirm = JOptionPane.showConfirmDialog(
                    AdminPanelGUI.this,
                    "Bạn có muốn ẩn Admin Panel?\n(Panel vẫn sẽ chạy trong background)",
                    "Xác nhận",
                    JOptionPane.YES_NO_OPTION
                );
                if (confirm == JOptionPane.YES_OPTION) {
                    setVisible(false);
                }
            }
        });

        // Main panel
        JPanel mainPanel = new JPanel();
        mainPanel.setLayout(new BorderLayout(10, 10));
        mainPanel.setBorder(BorderFactory.createEmptyBorder(15, 15, 15, 15));
        mainPanel.setBackground(new Color(240, 240, 240));

        // ===== PHẦN THÔNG TIN SERVER =====
        JPanel infoPanel = new JPanel();
        infoPanel.setLayout(new GridLayout(8, 1, 5, 5));
        infoPanel.setBorder(BorderFactory.createTitledBorder(
            BorderFactory.createLineBorder(new Color(70, 130, 180), 2),
            "📊 THÔNG TIN SERVER",
            0,
            0,
            new Font("Arial", Font.BOLD, 14),
            new Color(70, 130, 180)
        ));
        infoPanel.setBackground(Color.WHITE);

        lblServerName = createInfoLabel("Server: " + ServerManager.NAME);
        lblStatus = createInfoLabel("Status: 🟢 Đang hoạt động");
        lblPlayersOnline = createInfoLabel("Players Online: 0 / " + Manager.MAX_PLAYER);
        lblExpRate = createInfoLabel("EXP Rate: x" + Manager.RATE_EXP_SERVER);
        lblUptime = createInfoLabel("Uptime: " + ServerManager.timeStart);
lblCpu = createInfoLabel("CPU: 0%");
lblRam = createInfoLabel("RAM: 0 MB / 0 GB");
lblThreads = createInfoLabel("Threads: 0");
        infoPanel.add(lblServerName);
        infoPanel.add(lblStatus);
        infoPanel.add(lblPlayersOnline);
        infoPanel.add(lblExpRate);
        infoPanel.add(lblUptime);
        infoPanel.add(lblCpu);
infoPanel.add(lblRam);
infoPanel.add(lblThreads);

        // ===== PHẦN ĐIỀU KHIỂN =====
        JPanel controlPanel = new JPanel();
        controlPanel.setLayout(new GridLayout(4, 1, 10, 10));
        controlPanel.setBorder(BorderFactory.createTitledBorder(
            BorderFactory.createLineBorder(new Color(255, 140, 0), 2),
            "⚙️ ĐIỀU KHIỂN",
            0,
            0,
            new Font("Arial", Font.BOLD, 14),
            new Color(255, 140, 0)
        ));
        controlPanel.setBackground(Color.WHITE);

        // Row 1: Bảo trì 20s + Kick All
        JPanel row1 = new JPanel(new GridLayout(1, 2, 10, 0));
        row1.setBackground(Color.WHITE);

        btnMaintenance20s = createButton("⏰ BẢO TRÌ 20S", new Color(255, 200, 0));
        btnMaintenance20s.addActionListener(e -> onMaintenance20s());

        btnKickAll = createButton("👢 KICK ALL", new Color(220, 20, 60));
        btnKickAll.addActionListener(e -> onKickAll());

        row1.add(btnMaintenance20s);
        row1.add(btnKickAll);

        // Row 2: Bảo trì ngay + Làm mới
        JPanel row2 = new JPanel(new GridLayout(1, 2, 10, 0));
        row2.setBackground(Color.WHITE);

        btnMaintenanceNow = createButton("⚡ BẢO TRÌ NGAY", new Color(255, 140, 0));
        btnMaintenanceNow.addActionListener(e -> onMaintenanceNow());

        btnRefresh = createButton("🔄 LÀM MỚI", new Color(70, 130, 180));
        btnRefresh.addActionListener(e -> updateInfo());

        row2.add(btnMaintenanceNow);
        row2.add(btnRefresh);

        // Row 3: Thay đổi EXP
        JPanel row3 = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
        row3.setBackground(Color.WHITE);

        JLabel lblExp = new JLabel("⭐ Thay đổi EXP:");
        lblExp.setFont(new Font("Arial", Font.BOLD, 13));

        String[] expOptions = {"x1", "x2", "x5", "x10", "x20", "x30", "x40", "x50"};
        cboExpRate = new JComboBox<>(expOptions);
        cboExpRate.setFont(new Font("Arial", Font.PLAIN, 13));
        cboExpRate.setPreferredSize(new Dimension(100, 30));

        btnApplyExp = createButton("✅ ÁP DỤNG", new Color(34, 139, 34));
        btnApplyExp.setPreferredSize(new Dimension(120, 30));
        btnApplyExp.addActionListener(e -> onApplyExp());

        row3.add(lblExp);
        row3.add(cboExpRate);
        row3.add(btnApplyExp);

        // Row 4: Thông báo
        JLabel lblNote = new JLabel("💡 Panel tự động cập nhật mỗi 2 giây");
        lblNote.setFont(new Font("Arial", Font.ITALIC, 11));
        lblNote.setForeground(Color.GRAY);
        lblNote.setHorizontalAlignment(SwingConstants.CENTER);

        controlPanel.add(row1);
        controlPanel.add(row2);
        controlPanel.add(row3);
        controlPanel.add(lblNote);

        // Add panels to main panel
        mainPanel.add(infoPanel, BorderLayout.NORTH);
        mainPanel.add(controlPanel, BorderLayout.CENTER);

        add(mainPanel);
    }

    private JLabel createInfoLabel(String text) {
        JLabel label = new JLabel(text);
        label.setFont(new Font("Arial", Font.PLAIN, 13));
        label.setBorder(BorderFactory.createEmptyBorder(5, 10, 5, 10));
        return label;
    }

    private JButton createButton(String text, Color color) {
        JButton button = new JButton(text);
        button.setFont(new Font("Arial", Font.BOLD, 13));
        button.setBackground(color);
        button.setForeground(Color.WHITE);
        button.setFocusPainted(false);
        button.setBorder(BorderFactory.createRaisedBevelBorder());
        button.setCursor(new Cursor(Cursor.HAND_CURSOR));

        // Hover effect
        button.addMouseListener(new MouseAdapter() {
            @Override
            public void mouseEntered(MouseEvent e) {
                button.setBackground(color.brighter());
            }

            @Override
            public void mouseExited(MouseEvent e) {
                button.setBackground(color);
            }
        });

        return button;
    }

    private void updateInfo() {
        try {
            int playerCount = Client.gI().getPlayers().size();
            lblPlayersOnline.setText("Players Online: " + playerCount + " / " + Manager.MAX_PLAYER);
            lblExpRate.setText("EXP Rate: x" + Manager.RATE_EXP_SERVER);
            lblStatus.setText("Status: " + (Maintenance.isRunning ? "🔴 Đang bảo trì" : "🟢 Đang hoạt động"));
            lblUptime.setText("Uptime: " + ServerManager.timeStart);
            // CPU
Runtime runtime = Runtime.getRuntime();
long usedMemory = runtime.totalMemory() - runtime.freeMemory();
long maxMemory = runtime.maxMemory();
double cpuUsage = (double) usedMemory / maxMemory * 100;
lblCpu.setText(String.format("CPU: %.1f%%", cpuUsage));

// RAM (JVM)
long usedMB = usedMemory / 1024 / 1024;
long maxMB = maxMemory / 1024 / 1024;
lblRam.setText(String.format("RAM: %d MB / %d MB (JVM)", usedMB, maxMB));

// Threads
int threadCount = Thread.activeCount();
lblThreads.setText("Threads: " + threadCount);
        } catch (Exception e) {
            Logger.error("Lỗi cập nhật thông tin Admin Panel: " + e.getMessage());
        }
    }

    private void startAutoUpdate() {
        updateTimer = new Timer(2000, e -> updateInfo());
        updateTimer.start();
    }

    // ===== CÁC CHỨC NĂNG ĐIỀU KHIỂN =====

    private void onMaintenance20s() {
        int confirm = JOptionPane.showConfirmDialog(
            this,
            "⚠️ XÁC NHẬN BẢO TRÌ 20 GIÂY ⚠️\n\n" +
            "• Server sẽ countdown từ 20→1 giây\n" +
            "• Thông báo gửi cho tất cả player\n" +
            "• Server tự động tắt sau 20s\n\n" +
            "Bạn có chắc chắn?",
            "Xác nhận bảo trì",
            JOptionPane.YES_NO_OPTION,
            JOptionPane.WARNING_MESSAGE
        );

        if (confirm == JOptionPane.YES_OPTION) {
            Maintenance.gI().startSeconds(20);
            JOptionPane.showMessageDialog(
                this,
                "✅ Đã kích hoạt bảo trì 20 giây!\n⏰ Countdown bắt đầu...",
                "Thành công",
                JOptionPane.INFORMATION_MESSAGE
            );
            Logger.log(Logger.YELLOW, "[ADMIN PANEL] Đã kích hoạt bảo trì 20 giây");
        }
    }

    private void onKickAll() {
        int confirm = JOptionPane.showConfirmDialog(
            this,
            "⚠️ XÁC NHẬN ĐÁ ALL PLAYER ⚠️\n\n" +
            "• Tất cả player bị kick NGAY\n" +
            "• Server tắt NGAY LẬP TỨC\n" +
            "• KHÔNG CÓ COUNTDOWN!\n\n" +
            "Bạn có chắc chắn?",
            "Xác nhận kick all",
            JOptionPane.YES_NO_OPTION,
            JOptionPane.ERROR_MESSAGE
        );

        if (confirm == JOptionPane.YES_OPTION) {
            Maintenance.gI().startImmediately();
            JOptionPane.showMessageDialog(
                this,
                "✅ Đang kick all players...\n⚡ Server sẽ tắt ngay!",
                "Thành công",
                JOptionPane.INFORMATION_MESSAGE
            );
            Logger.log(Logger.YELLOW, "[ADMIN PANEL] Đã kick all players");
        }
    }

    private void onMaintenanceNow() {
        int confirm = JOptionPane.showConfirmDialog(
            this,
            "⚠️ BẢO TRÌ NGAY (5 GIÂY COUNTDOWN) ⚠️\n\n" +
            "Server sẽ tắt sau 5 giây.\n\n" +
            "Bạn có chắc chắn?",
            "Xác nhận bảo trì",
            JOptionPane.YES_NO_OPTION,
            JOptionPane.WARNING_MESSAGE
        );

        if (confirm == JOptionPane.YES_OPTION) {
            Maintenance.gI().startSeconds(5);
            JOptionPane.showMessageDialog(
                this,
                "✅ Đã kích hoạt bảo trì 5 giây!",
                "Thành công",
                JOptionPane.INFORMATION_MESSAGE
            );
            Logger.log(Logger.YELLOW, "[ADMIN PANEL] Đã kích hoạt bảo trì 5 giây");
        }
    }

    private void onApplyExp() {
        String selected = (String) cboExpRate.getSelectedItem();
        int newExpRate = Integer.parseInt(selected.replace("x", ""));

        int confirm = JOptionPane.showConfirmDialog(
            this,
            "Bạn có muốn đổi EXP Server thành " + selected + "?\n\n" +
            "Thay đổi sẽ áp dụng NGAY cho tất cả player!",
            "Xác nhận đổi EXP",
            JOptionPane.YES_NO_OPTION,
            JOptionPane.QUESTION_MESSAGE
        );

        if (confirm == JOptionPane.YES_OPTION) {
            int oldRate = Manager.RATE_EXP_SERVER;
            Manager.RATE_EXP_SERVER = (byte) newExpRate;

            // Gửi thông báo cho tất cả player
            nro.models.services.Service.gI().sendThongBaoAllPlayer(
                "🎉 THÔNG BÁO 🎉\n" +
                "EXP server đã thay đổi!\n" +
                "Từ x" + oldRate + " → x" + newExpRate + "\n" +
                "Chúc các bạn luyện cấp vui vẻ!"
            );

            updateInfo();

            JOptionPane.showMessageDialog(
                this,
                "✅ Đã thay đổi EXP thành công!\n" +
                "📊 Từ: x" + oldRate + " → x" + newExpRate + "\n" +
                "⚡ Đã thông báo cho tất cả player!",
                "Thành công",
                JOptionPane.INFORMATION_MESSAGE
            );

            Logger.log(Logger.YELLOW, "[ADMIN PANEL] Đã đổi EXP từ x" + oldRate + " → x" + newExpRate);
        }
    }

    // ===== STATIC METHOD ĐỂ MỞ PANEL =====

    public static void openAdminPanel() {
        SwingUtilities.invokeLater(() -> {
            AdminPanelGUI panel = new AdminPanelGUI();
            panel.setVisible(true);
            Logger.log(Logger.GREEN, "[ADMIN PANEL] Đã mở GUI Admin Panel");
        });
    }
}