   M S B A M L   `   `   `  ����?  ;chat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Chat.ViewModelChat.ViewModel  5   	       Chat.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2	viewmodelclr-namespace:Chat.ViewModel     5   	     � Title$  
MainWindow��5	   	   $	��600��5
      $	��900��5       � 
ResizeMode$ NoResize=�5      $��18(�5       � DataContext 5      "   Chat.ViewModel.MainViewModel  5   
    � 	Resources 5         MyConverter��%   &	        �� 5   
   6'   5      .��5      �� ���#191919._�5   
   �� .+�5      ��  �� Orientation$ 
Horizontal=�5       �� Left$	 230��5      
 �� Top$ 30��5      .+�5      	� -   5         	�Name$ MyJoin��5       d� 	IsChecked$
 TrueY�5      
�������$
��Host��5!        RadioButtonTheme#��[ 5"      $��Center=�5#      $	��145��5$      	� 5'      -   5        $ MyHost��5(      
�������$�� Join��5*      #��[ 5+      $��56��5,      $	��145��5-      $��Center=�5.      	� 50      -   5        $ 	MyHistory��51      
�������$��History��53      #��[ 54      $��56��56      $��Center=�57      $	��145��58      ��55      �� *	History+58      5:      �� 5<      $��	Username:��5=      $��Right=�5>      
�������	 J� RenderTransformOrigin$	 0.953,0.366&�5@      $	 271��5A      $	 199��5B      $��38��5C      �� 5E      $
��Left=�5H      $	��200��5I      
����������#232323$	 370��5L      $	 199��5M      $��38��5N      ��5F      �� *User.Username+
 �� UpdateSourceTrigger$
 PropertyChanged=�5N      �� 5Q      $��Port:��5R      $��Right=�5S      
�������$	 293��5U      $	 295��5V      $	 1.696,0.735&�5W      $��38��5X      �� 5Z      $
��Left=�5]      $	��200��5^      
����������#232323$	 370��5a      $	 295��5b      $��38��5c      ��5[      �� *	User.Port+$
 PropertyChanged=�5c      �� 5e      $��
IP-adress:��5f      $��Right=�5g      
�������$	 276��5i      $	 247��5j      $��38��5k      �� 5m      $
��Left=�5n      $	��200��5o      
����������#232323$	 370��5t      $	 247��5u      $��38��5v      ��5p      �� *User.Addres+$
 PropertyChanged=�5v      �� 5x      $��Connect��6   $
��Left=�5z      $	��Top=�5{      ���#1b3185
�������$��56��5�      $	��145��5�      $	 384��5�      $	 372��5�      ��5y      �� *	Connect+y�5      ��  �� ElementName$ MyHost�� �� Path$ 	IsChecked� �� 	Converter# [  5�      �� 5�      $��Listen��6   $
��Left=�5�      $	��Top=�5�      ���#1b3185
�������$��56��5�      $	��145��5�      $	 384��5�      $	 372��5�      ��5�      �� *Listen+y�5�      �� $ MyJoin��$ 	IsChecked�# [  5�      �� 5�      $	��571��5�      $	��894��5�      ��5�      �� *
ChatView+5�      �� 5�      $	��430��5�      $	��894��5�      $	 141��5�      ��5�      �� *HistoryView+y�5�      �� $ 	MyHistory��$ 	IsChecked�# [  5�      5�      5�      