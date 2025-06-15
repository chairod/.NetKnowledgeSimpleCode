#### เว็บที่ใช้ทดสอบ Regular expression  
https://regex101.com/  

### รูปแบบการเขียน Positive Lookbehind  รูปแบบ (?<=Pattern ที่ต้องการเขียน)
**ตัวอย่าง**  
```
(?<=\d)\s{2,}
```

Regular expression จะมองหาข้อความที่อยู่ด้านหลัง Pattern ที่กำหนด โดยไม่รวมเอา Character Group ที่ตรงกับ Pattern เข้าไปในชุด Macthing string นั้นๆ  
ตัวอย่างข้อความ
```
1000 abcd 150221  22352
```
![image](https://github.com/user-attachments/assets/56bb6e6a-fa2e-41c7-8189-d9149e282540)





### รูปแบบการเขียน Positive Lookahead  (?=Pattern ที่ต้องการเขียน)  
ตัวอย่าง  
```
[a-z](?=\d+)
```
Regular Expression จะมองหาตัว อักขระภาษาอังฤฤษ a-z ที่อยู่ด้านหน้าตัวเลขตั้งแต่ 1 ตัวขึ้นไป และจะไม่นำตัวเลขที่ Match กับ Pattern ไปรวมไว้ในผลลัพธ์ของ String  
ตัวอย่างข้อความ
```
test data 12345, ddfwddfsdf2234 
```
![image](https://github.com/user-attachments/assets/5c2bbebd-a89d-4100-85b8-bdb49ea2318c)



