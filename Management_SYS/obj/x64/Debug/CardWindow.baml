   M S B A M L   `   `   `  ����  Management_SYS    Management_SYS.CardWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  
CardWindow��5   	   $	��500��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ ContactHistoryListView��5      $��10q�63   $	��Top=�6?   $��0��6W    �� View 5      �� 5      .P�5      �� $��NameOfCustomer��6%    �� Width$
 Auto��6m    �� DisplayMemberBinding 6=   �� *NameOfCustomer+6m   �� 5      $��PhoneNumberOfCustomer��6%   $
 Auto��6{    6D   �� *PhoneNumberOfCustomer+6{   �� 5      $��Contact_info��6%   $
 Auto��6i    6;   �� *Contact_info+6i   �� 5      $��Time_of_contact��6%   $
 Auto��6o    6>   �� *Time_of_contact+6o   5      5      � 5   
   $��1��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5"      �� -   5         ��Name$ ContactInfoTextBox��5"      $	��200��62   $��	0,0,284,0q�6A   &   Xceed.Wpf.Toolkit.DateTimePicker  5#      -   5          Name$ NextContactDatePicker��5#   "   $��Right=�6A   $��Center=�6]   $	��150��6x   ,  &Xceed.Wpf.Toolkit.Primitives.InputBase	   	Watermark$	 Select contact��6�   $��
552,0,78,0q�6�   5$      �� 5&   
   -   5        
 ��Name$
 AddContactButton��5&      $	��Add��6+   $��10q�69   $��Right=�6E   $��Center=�6a   $��1��6�   �� 5'   
   -   5        $
 
ExitButton��5'      $
��Exit��6%   $��10q�64   $��Right=�6@   $	��Top=�6\   6t   5(      5)      