   M S B A M L   `   `   `  ����  	TourGuide3    -TourGuide.PresentationLayer.Views.AddTourView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Add Tour��5   	   $	��540��6   $	��400��6'   $	��300��5   	   $	��400��6   .��5      �� $
��Auto=�6   .��5   
   � $��20q�6    � RowDefinitions 5	      �� 5
      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��
Tour Name:��6   $��0��6*   $��5q�67   �� 5      $��1��6   $��25��5      $��5q�6"   ��5   #   �� *NewTour.name+ �� UpdateSourceTrigger$ PropertyChanged=�5   "   �� 5      $��Description:��6   $��2��6,   $��5q�69   �� 5       $��3��6   $��25��5!      $��5q�6"   ��5    #   �� *NewTour.description+$ PropertyChanged=�5!   "   �� 5#      $��Start Location:��6   $��4��6/   $��5q�6<   �� 5$      $��5��6   $��25��5%      $��5q�6"   ��5$   #   �� *NewTour.startLocation+$ PropertyChanged=�5%   "   �� 5'      $��End Location:��6   $��6��6-   $��5q�6:   �� 5(      $��7��6   $��25��5)      $��5q�6"   ��5(   #   �� *NewTour.endLocation+$ PropertyChanged=�5)   "   �� 5+      $��Transport Type:��6   $��8��6/   $��5q�6<   �� 5,      $��9��6   $��25��5.      $��5q�6#   ��5,   $   �� *TransportTypes+ �� SelectedValue 5-      �� *NewTour.transporttype+ �� Mode$ TwoWay=�5.   #   �� 52      $��16��6   $��Center=�6'   $��10q�6D   .+�53      �� $��💾 Save Tour��6   ���#27AE60
�������$	��180��56      $��40��6%   $��14(�61    � Cursor$
 Hand��57      $
��Bold"�6'   ��54      �� *SaveTourCommand+57   '   58      59      5:      5;      