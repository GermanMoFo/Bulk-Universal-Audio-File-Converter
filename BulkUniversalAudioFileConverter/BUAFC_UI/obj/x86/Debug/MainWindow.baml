   M S B A M L   `   `   `  ����L  HBUAFC_UIPrototype, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BUAFC_UIBUAFC_UI  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   @       BUAFC_UI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:BUAFC_UI     5   	   X TXceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   3System&clr-namespace:System;assembly=mscorlib  6@     � Title$1  +BUAFC - Bulk Universal Audio File Converter��5	   	   $	��535��6=   $	��950��6J   $
��Left=�6V   $	��Top=�6q   $	��950��6�   $	��545��6�    � 	Resources 5
         TreeViewItemStyle �� 
TargetType �� Property �� Value �� Mode-  'System.Windows.Input.KeyboardNavigation  CheckBoxItemTemplate �� ItemsSource �� Orientation d� 	IsChecked�� 5   
   %$  &	        &	     �� 5      " Q�6.   .�5      ��  �@Q�
IsExpanded . 6/   �� 5       �@Q�
IsSelected 6/   �� *IsInitiallySelected+$ OneTime=��� 5       �@ AcceptsReturn . 6E   �� 5       �@�ToolTip 6,   �� *	Tooltip+5      �� 5       6D   �� *
Children+$ OneTime=�.R�5      �� $ 
Horizontal=�6   .+�5      �� |�.  $��Center=�6Q    61   �� *	IsChecked+6Q   �� 5      $	��2,0q�6N   ��6'   �� *Name+$ OneTime=�6N   5      5      5      .��5      � $��0,0,-6.667,7.667q�6   	 � RowDefinitions	 5   
   �� 5      
 � ColumnDefinitions
 5   
   �� 5      $
��377* �6   �� 5       $
��193* �6   $	��200��6,   �� 5!      $
��365* �6   .+�5#   
   �� $��103.333,32,89.333,0q�6   $��Center=�6.   $	��Top=�6K   $��0��6c   $��0��6n   $��1��6x   � 5$   
   $��4.667,10,16.333,0q�6   $��Center=�6*   $	��175��6G   $��1��6S   .+�5%      �� $��To...��6   $��Center=�6$   $��
4,87,120,0q�6A   $	��Top=�6U   $��51��6m   $��39��6x   $��20(�6�   �� 5&      -   5         ��Name$ CMBB_TO��5&      $��Center=�6(   $��10,118,76,0q�6E   $	��Top=�6Z   $��89��6r    � ToolTip$  File format to convert to.��6�   �� 5'      $��From...��6   $��Center=�6&   $��4,0,89,0q�6C   $	��Top=�6U   $��82��6m   $��39��6x   $��20(�6�   	� 5(      -   5         	�Name$ 	RB_STRICT��5(      $�� ��6-   $��Center=�68   $	��Top=�6U   $��22��6�   $��
4,60,134,0q�6�   $��37��6�   $) #Filter selection based on extension��6�   	� 5)      -   5        $ RB_ALL��5)      $��All...��6*   $��Center=�6;   $��
2,37,105,0q�6X   $	��Top=�6l   $��19��6�   $��68��6�   $
 TrueY�6�   $) #Entire selection will be converted.��6�   �� 5*      $��Options��6   $��Center=�6(   $	��334��6E   $��
10,154,6,0q�6R   $	��Top=�6f   $	��159��6~   .��5+      � $��-1,0,5,-13.333q�6   $��Center=�6/   $	��Top=�6L   .+�5,      �� $��Original Files��6   $
��Left=�67   $��68��6R   $	��Top=�6^   $	��128��6v   $��	10,10,0,0q�6�   .��5-      � $��4,4,-2,0q�6$    � Orientation$ Vertical=�66   $��Center=�6M   $	��Top=�6j   .+�5.      	� -   5        $ 	RB_DELETE��5.   *   $��Delete Originals��6=   $
��Left=�6X   $	��Top=�6s   $
 TrueY�6�   6�   	� 5/      -   5        $��Preserve Originals��5/   *   $
��Left=�6G   $	��Top=�6b   $
 TrueY�6z   6�   50      51      �� 52      $��
Desination��6   $
��Left=�63   $	��110��6N   $	��Top=�6[   $	��128��6s   $��10,88,0,-95.333q�6   $# Where to put converted files.��6�   .��53      �� -   5         ��Name$ BUT_BROWSEDEST_Copy��53   !   $��	Browse...��6>   $��Right=�6R   $��	0,61,32,0q�6n   $	��Top=�6�   $��57��6�   $��23��6�   $. (Select a directory from a dialog window.��6�   {�.  6   54      �� 55      $��Encoding Options��6   $
��Left=�69   $��98��6T   $	��Top=�6`   $	��128��6x   $��10,210,0,-205.333q�6�   .��56      � $��-6,-8,-39,-6.333q�6   .+�57      %  Xceed.Wpf.Toolkit.IntegerUpDown  -   5          Name$ 
IUD_SAMPLE��57   1   $��Center=�6E   $��
35,65,53,0q�6b   $	��Top=�6v   $��73��6�   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value$
 1024X�6�   6�     58      -   5        $ IUD_BITRATE��58   1   $
��Left=�6F   $��	35,27,0,0q�6a   $	��Top=�6t   $��73��6�   $ 192000X�6�   6�   �� 59      $��OGG File Sample Size:��6$   $
��Left=�6D   $��	0,43,-1,0q�6_   $	��Top=�6r   $	��162��6�   �� 5:      $��Encoding Bit-Rate:��6$   $
��Left=�6A   $��4,5,0,0q�6\   $	��Top=�6m   $	��114��6�   $��27��6�   5;      5<      5>      5?      %  Xceed.Wpf.Toolkit.CheckComboBox  5@      -	   5          Name$ CCB_FROM��5@   !   $
��Left=�63   $��26��6N   $��	24,54,0,0q�6Z   $	��Top=�6m   $	��143��6�   $) #Filter selection based on extension��6�   6�   5A      �� 5B   
   -
   5        $  BUT_BROWSEPRIMARYDIRECTORY��5B      $��	Browse...��65   $��Right=�6I   $��0,34,0,0q�6e   $	��Top=�6w   $��57��6�   $��23��6�   $. (Select a directory from a dialog window.��6�   �� 5C   
   -   5         ��Name$ TXTB_PRIMARYDIRECTORY��5C      $��23��61   $��2,34,57.333,0q�6=    �� TextWrapping$
 Wrap=�6T   $��TextBox��6h   $	��Top=�6w   $	��318��6�   $��23��6�   .��6�    5D      �� 5E       The directory displayed below.�� 5G      A?After typing a new directory, press enter to confirm selection.5I      .��5J      5K      �� 5L   
   $��
Convert...��6   $
��Left=�6%   $	��108��6@   $��39��6L   $��20(�6X   $��-1,2,0,0q�6f   $	��Top=�6x   �� 5M   
   -   5        $ BUT_REFRESH��5M      $��Refresh��6&   $
��Left=�68   $��2,0,0,1q�6S   $��75��6d   $��Bottom=�6�   $��20��6�   $4 .Force a refreshed view of the targeted folder.��6�   �� 5N   
   -   5        $��Confirm And Convert��5N      $
��Left=�6/   $��1.667,0,0,7.667q�6J   $��Bottom=�6c   $	��177��6~   $��30��6�   $��18(�6�   $��2��6�   �� 5O   
   -   5        $ BUT_DISPLAYLESS��5O      $��>��6*   $��23.667,30,0,0q�69   $	��Top=�6n   $��27��6�   $
��Left=�6�   $��24��6�   $��2��6�   $ Display more.��6�   �� 5P   
   $��Check...��6   $
��Left=�6#   $	��Top=�6>   $	��189��6V   $��39��6b   $��20(�6n   $��50.667,22,0,0q�6|   $��2��6�   �� 5Q   
   -   5        $ BUT_DISPLAYMORE��5Q      $��<��6*   $��1.667,30,0,0q�69   $
��Left=�6m   $��24��6�   $	��Top=�6�   $��27��6�   $��2��6�   $ Display less.��6�   R� 5R   
   -   5         R�Name$ TV_FROM��5R      $��2,55,0.333,22q�6&   $	��350��6=   $	��400��6L   #��[  5S      #��[ 5T       J� RenderTransformOrigin$ 0,0.5&�6H    J� IsManipulationEnabled . 6f   �� 5U   
   -   5         ��Name$ LB_CHECK��5U      $��1.667,55,15,36.333q�6$   $ 0.5,0.5&�6@   $	��350��6`   $	��400��6o   $��2��6      BUAFC_UI.LoadingAnimation  5V   
   -   5          Name$ LD_FROM��5V   !   $��Center=�62   $��149,221,146.333,199q�6O   $��Center=�6l   $y�Hidden=�6�   �� 5W   
   -   5        $ TXT_DEST��5W      $��1��6$   $
��Left=�64   $��23��6O   $��36.667,320,0,0q�6[   $
 Wrap=�6s   $	��...��6�   $	��Top=�6�   $	��120��6�   {�.   �� TextAlignment$ Right=�6�   .��6   5X      �� 5Y      Folder to place output.�� 5[      Press "Enter"; to confirm.5]      .��5^      5_      �� 5`   
   -   5        $ CB_DEST��5`      $
��Left=�6$   $��36.667,293,0,0q�6?   $	��Top=�6W   $	��120��6o    �� SelectedIndex$ 0��6�   $��1��6�    5a      �� 5b      86In-Place - Files will apear alongside their originals.�� 5d      FDDirectory Dump - All outpt will be put into the specified directory.�� 5f      RPSmart Dump - A varient of Direcctory Dump where folder structure is also copied.5h      5j      5l      5m      