   M S B A M L   `   `   `  ����D  @TourGuide, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:TourGuide.PresentationLayer.Views!TourGuide.PresentationLayer.Views  5      6    0TourGuide.PresentationLayer.Views.TourDetailView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:TourGuide.PresentationLayer.Views     5      $
��Auto��5   7   $
��Auto��6E   .��5	      � $��20q�6     � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��
Tour Name:��6   $��0��6&   $��0��63   $
��Bold"�6C   �� 5   
   $��0��67   $��1��6D   $��10,0,0,0q�6T   ��6   �� *SelectedTour.name+6T   �� 5   
   $��Description:��6   $��1��6(   $��0��65   $
��Bold"�6E   �� 5   
   $��1��6<   $��1��6I   $��10,0,0,0q�6Y    �� 
IsReadOnly$
 True��6k   ��6   �� *SelectedTour.description+6k   �� 5   
   $��Start Location:��6   $��2��6+   $��0��68   $
��Bold"�6H   �� 5    
   $��2��6>   $��1��6K   $��10,0,0,0q�6[   $
 True��6m   ��6   �� *SelectedTour.startLocation+6m   �� 5"   
   $��End Location:��6   $��3��6)   $��0��66   $
��Bold"�6F   �� 5#   
   $��3��6<   $��1��6I   $��10,0,0,0q�6Y   $
 True��6k   ��6   �� *SelectedTour.endLocation+6k   �� 5%   
   $��Transport Type:��6   $��4��6+   $��0��68   $
��Bold"�6H   �� 5&   
   $��4��6>   $��1��6K   $��10,0,0,0q�6[   $
 True��6m   ��6   �� *SelectedTour.transporttype+6m   �� 5(   
   $��	Distance:��6   $��5��6%   $��0��62   $
��Bold"�6B   �� 5)   
   $��5��69   $��1��6F   $��10,0,0,0q�6V   $
 True��6h   ��6   �� *SelectedTour.distance+6h   �� 5+   
   $��7��6   $��1��6"    �� Orientation$ 
Horizontal=�62   $��Right=�6K   .+�5,      �� $��Modify��6   $	��100��6H   $��10q�6T   ��6&   �� *ModifyCommand+6T   �� 5-      $��Delete��6   $	��100��6H   $��10q�6T   ��6&   �� *DeleteCommand+6T   5.      5/      50      