-- Chèn dữ liệu vào bảng informations
INSERT INTO informations (id, name, birth, gender, phone, address, citizen_id)
VALUES
(22, 'Nguyen Van U', '03/12/1995', 'Male', '0901122334', 'Ha Noi', 1021),
(23, 'Le Thi V', '07/09/1992', 'Female', '0912456789', 'Sai Gon', 1022),
(24, 'Pham Van W', '12/05/1998', 'Male', '0923344556', 'Da Nang', 1023),
(25, 'Tran Thi X', '05/03/1996', 'Female', '0932233445', 'Hai Phong', 1024),
(26, 'Bui Van Y', '08/12/1993', 'Male', '0941122334', 'Can Tho', 1025),
(27, 'Ho Thi Z', '02/05/1999', 'Female', '0952233445', 'Nha Trang', 1026),
(28, 'Vu Van AA', '06/12/1997', 'Male', '0963344556', 'Hue', 1027),
(29, 'Ngo Thi BB', '09/12/2000', 'Female', '0974455667', 'Vung Tau', 1028),
(30, 'Dang Van CC', '11/08/1994', 'Male', '0985566778', 'Bien Hoa', 1029),
(31, 'Do Thi DD', '01/12/1991', 'Female', '0996677889', 'Thanh Hoa', 1030);

-- Chèn dữ liệu vào bảng users
INSERT INTO users (id, email, password, role, status, avatar, information_id, score, total_score)
VALUES
(22, 'user22@example.com', 'password22', 3, 1, '', 22, 100, 500),
(23, 'user23@example.com', 'password23', 3, 2, '', 23, 150, 600),
(24, 'user24@example.com', 'password24', 3, 1, '', 24, 200, 700),
(25, 'user25@example.com', 'password25', 3, 2, '', 25, 250, 800),
(26, 'user26@example.com', 'password26', 3, 1, '', 26, 300, 900),
(27, 'user27@example.com', 'password27', 3, 2, '', 27, 350, 1000),
(28, 'user28@example.com', 'password28', 3, 1, '', 28, 400, 1100),
(29, 'user29@example.com', 'password29', 3, 2, '', 29, 450, 1200),
(30, 'user30@example.com', 'password30', 3, 1, '', 30, 500, 1300),
(31, 'user31@example.com', 'password31', 3, 2, '', 31, 550, 1400);
