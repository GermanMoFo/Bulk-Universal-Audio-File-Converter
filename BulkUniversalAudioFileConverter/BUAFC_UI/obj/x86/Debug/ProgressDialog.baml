   M S B A M L   `   `   `  ����L  HBUAFC_UIPrototype, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BUAFC_UIBUAFC_UI  5   	       BUAFC_UI.ProgressDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:BUAFC_UI     5   	   $	��141��5      $��491.667��6'     � Title$  Conversion In Progress��67   $	��150��5   	   $
��1000��5	   	   $	��500��6   $	��150��6(   .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��317* �6   �� 5      $
��168* �6   $	��170��6,   $	��170��6;   .+�5   
   � -   5         �Name$ PRGBR_PROGRESS��5      $��10,0,0.333,10q�6.   $	��300��6E   $��25��6T   $��Bottom=�6c   �� 5   
   -   5         ��Name$ TXTB_Action��5      $5��/STATUS TXT - REPORTS CURRENT ACTION ONTO A FILE��6%   $	��Top=�6_   $��0,45,0,0q�6w   $
��Left=�6�   $	��485��6�   $��2��6�   �� 5   
   -   5         ��Name$ 
BUT_CANCEL��5      $��Cancel��6%   $��Right=�66   $��Bottom=�6R   $��75��6m   $��0,0,10.333,10q�6x   $��1��6�    � ToolTip$V PWill attemp to terminate process, may take a moment to safetly exit the process.��6�   �� 5   
   -   5        $ TXTB_Directory��5      $-��'STATUS TXT - REPORTS CURRENT DIRECTORY ��6(   $	��Top=�6Z   $��0,14,0,0q�6r   $
��Left=�6�   $	��485��6�   $��2��6�   �� 5   
   -   5        $ BUT_OKAY��5      $
��Okay��6#   $��Right=�62   $��Bottom=�6N   $��75��6i   $��0,0,90.333,10q�6t   {�.  $��1��6�   �� 5   
   -   5        $ 	TXTB_Prog��5      $)��#STATUS TXT - REPORTS PROGRESS/TOTAL��6#   $��Bottom=�6Q   $��	10,0,0,10q�6l   $	��300��6   $��25��6�   $	��305��6�   5      5      