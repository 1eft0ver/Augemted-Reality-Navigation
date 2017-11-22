# AR Navi
This is a graduation project of CSE 107th, NSYSU,
developed by 蔡宜勲(1eft0ver), 林鴻丞(jimmylin1017) and 劉昀鑫.

# 2017/11/22
Graduation project exhibition.
Final version project uploaded.

# 2017/10/31

* 新增 labelDistance 的顯示顏色會根據距離改變
	* 50|100|200|300|500 -> red|yellow|green|cyan|blue
	* 其餘皆 black
* 新增 labelContent
* 將全部的 Scene 的 BackButton 移除
	* Label Choose Scene 沒有移除，因為 Button 除了 Back 還有 Save 的功能
	* 改用 `KeyCode.Escape`
```
if (Input.GetKeyDown(KeyCode.Escape))
{

}
```
* 解決 Label 在螢幕中間會被切掉的問題
	* 將 LabelCamera 的 Far 從 1000 -> 1100
	* 因為 Label 的 Z 軸為 1000，在上下旋轉時會遇到死角

# 2017/10/30

* 新增下載 ZIP 和解壓縮 ZIP
* Map 架構 (**完成**)
	* 加入 isNode 的判斷
* Label 可以放圖片(Spirit)
	* 可以到 content 找到對應的圖片，目前只能用 JPG
* **缺少自訂圖片**
	* 研究 OpenFilePanel
* APP 開啟後自動創建的 temp.txt 改成 temp 資料夾

# 2017/10/23

* SetLabel 中，對應 Google Static Map 的 marker label {A-Z0-9} **完成**
	* GoogleMap
* LabelChoose 選擇特定的 Label 顯示 **完成**
* LabelChoose 中，對應 Google Static Map 的 marker label {A-Z0-9} **完成**
	* GoogleMapChoose
* 新增 TCP 連線 Script
* **缺少 Label 自訂圖片功能** (SetLabel 中)
	* 研究 OpenFilePanel
	* 研究 Txture2D
	* 研究 Spirit
* Map 架構
	* Map Folder (Map, Distance, Tag)
		* Image Folder


# 2017/10/04

* 新增在 SetLabel 中，刪除特定 label 的按鈕
* SetLabel 中，對應 Google Static Map 的 marker label {A-Z0-9} (**暫時拿掉，還在想表示方法**)
* LabelChoose 選擇特定的 Label 顯示 (**半成品**，只做完選單，LabelCreate 還沒做)
	* createLabel function 需要重新寫
	* Update function 中的應該可以搬過去用

# 2017/10/02

* 新增 UI 中，選擇檔案的 Dropdown
* 修改 LabelMain，增加全域 label 容器
* SetLabel 可以將 label 對應到 Google Static Map 中
* 新增檔案儲存命名功能
* **缺少 SetLabel 中，刪除特定 label 的按鈕**
* SetLabel 中，對應 Google Static Map 的 marker label {A-Z0-9} (**半成品**)

# 2017/09/25

* 新增 Google Static Map
* 程式架構需修改，需要多變數跨 Scene

# 2017/09/08

* 新增 `Button` 和 `InputField`
* 新增寫入檔案 `setLabel(string labelString)`
* 可成功抓取當下經緯度和自訂名稱
* Google Map 顯示 (**未開始**)
