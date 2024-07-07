# 1. Chạy Tầng Client (yêu cầu có nodejs v16 trở lên)
    ## 1.1. cd Client/QuanLyChay
    ## 1.2. npm install
    ## 1.3.1(nếu chạy trên web) npm start
    ## 1.3.2(build file apk) eas build -p android --profile preview
# 2. Chạy Tầng Server 
    ## 2.1 cd Server/FireManagerServer/FireManagerServer
    ## 2.2 Sửa các cấu hình trong appseting sao cho phù hợp
    ## 2.3 build trực tiếp bằng docker
     (docker build -t nameproject
    docker run -p 80:80 nameproject
     ) hoặc chạy solution public ra file tải file lên host.
# 3. Chạy tầng Device 
   ## 3.1 cd Device/FirePoject esp 32-1
   ## 3.2 Cấu hình lại Id và wifi của esp
   ## 3.3 sử dụng platform Io để nạp code vào thiết bị phần cứng