   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections  5   	   F BWpfTotalnik, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:WpfTotalnikWpfTotalnik 5   	      WpfTotalnik.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Bcol2clr-namespace:System.Collections;assembly=mscorlib     5   	   +localclr-namespace:WpfTotalnik     5   	   -   5          � Title$  Totalnik��5	   	   $��507.333��6   $
��1200��6+    � WindowStartupLocation$ CenterScreen=�68    � Icon$ Totalcmd.jpg��6]    � 
ResizeMode$ CanMinimize=�6�   .��5
      � $��	0,0,4,383q�6   .+�5   
   o�  o� 
IsMainMenu . $
��Left=�6!   $��22��6<   $��10,5,0,0q�6H   $	��Top=�6Z   $��54��6r   $��14(�6}   
�������.@�5      m� $��	_Файл��6   $��54��6&   .@�5      m� -   5        $'��!Сравнить каталоги��5      66   �� 5      m� 5      -   5        $��
Выход��5      6*   5      5      �� 5   
   -   5         ��Name$ textBoxLeft��5      $
��Left=�6'   $��20��6B   $��7,61,0,0q�6N    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��570��6�   $��12(�6�   ���	#FF93B2D6$��1q�6�   �� 5   
   -   5        $ textBoxRight��5      $
��Left=�6(   $��20��6C   $��
604,61,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $	��570��6�   $��12(�6�   ���	#FF93B2D6 �� 
IsReadOnly . 6�   � 5   
   $
��Left=�6   $	��350��6*   $��6,92,0,-355q�67   $	��Top=�6L   $	��981��6d   .+�5      �� -   5         ��Name$ leftListFiles��5      $
��Left=�6.   $	��575��6I   $��Disabled=�6U   $��0,0,0,10q�6�   ��5      �� 5      	 �� 
TargetType"	 ��6   5      
 �� View
 5      �� 5      5      5       �� 5!      -   5        $ rightListFiles��5!   y   $
��Left=�6   $	��340��62   $��598,0,-190,0q�6?   $	��Top=�6U   $	��570��6m   $��Disabled=�6�   .@�5"      �� 5#      5$      5%      a� 5&   
   -   5         a�Name$ 
leftDrives��5&      $
��Left=�6+   $��24��6F   $��	10,32,0,0q�6R   $	��Top=�6e   $	��577��6}    �� Orientation$ 
Horizontal=�6�   a� 5'   
   -	   5        $ rightDrives��5'      $
��Left=�6,   $��24��6G   $��
603,32,0,0q�6S   $	��Top=�6g   $	��577��6   $ 
Horizontal=�6�   5)      5*      