   M S B A M L   `   `   `  ����I  EManagement_SYS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Management_SYSManagement_SYS  5   	   "    Management_SYS.HistoryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Management_SYS     5   	     � Title$  History Window��5   	   $	��450��6    $	��800��6-    � 
ResizeMode$ 	CanResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	   .��5      ]� .+�5   
   �� $
��Left=�6      PrimaryHueLightBrush#���   6+   $��10q�6_   $��Stretch=�6k   $	��200��6�   .+�5      �� $��Contact History��6   $��24(�6/   $
��Bold"�6=   
�������$��10q�6b   �� 5      -   5        $��Back to Cabinet��5      $
��Left=�6L   $	��Top=�6g   $��
10,10,0,10q�6   $��	Underline{�6�    � Cursor$
 Hand��6�   $	��150��6�   
�������6�   5      �� 5   
   
�������$��10q�6$    �� Padding$ 10q�60    �� CornerRadius$ 5��6=   $��Stretch=�6N   $��Stretch=�6l   ._�5      �  � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $��* �6$   .+�5      �� $��Contact History��6   $��20(�63   $
��Bold"�6A   
�������$��0,0,0,20q�6e   $	��Top=�6w   $��Center=�6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ ListOfContacts��5         AutoGenerateColumns .  $��1��6O   $��0,10,0,0q�6\   	   SelectionMode$	 Single=�6n   
   
IsReadOnly
 .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Customer Name��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6g   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *NameOfCustomer+6g     5      $ Phone Number��6-   $ * 6m    6C   �� *PhoneNumberOfCustomer+6m     5       $ Contact Info��6-   $ * 6d    6C   �� *Contact_info+6d     5!      $ Contact Time��6-   $ * 6g    6C   �� *Time_of_contact+6g   5#      5$      5%      5&      5'      