   M S B A M L   `   `   `  ����  	TourGuide3    -TourGuide.PresentationLayer.Views.AddTourView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Add Tour��5   	   $	��540��6   $	��400��6'   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��
Tour Name:��6   $��0��6&   $��5q�63   �� 5   
   $��1��6   $��Stretch=�5      $��25��60   $��5q�6<   ��5      �� *NewTour.name+ �� UpdateSourceTrigger$ PropertyChanged=�5   <   �� 5   
   $��Description:��6   $��2��6(   $��5q�65   �� 5   
   $��3��6   $��Stretch=�5      $��25��60   $��5q�6<   ��5      �� *NewTour.description+$ PropertyChanged=�5   <   �� 5!   
   $��Start Location:��6   $��4��6+   $��5q�68   �� 5"   
   $��5��6   $��Stretch=�5#      $��25��60   $��5q�6<   ��5"      �� *NewTour.startLocation+$ PropertyChanged=�5#   <   �� 5%   
   $��End Location:��6   $��6��6)   $��5q�66   �� 5&   
   $��7��6   $��Stretch=�5'      $��25��60   $��5q�6<   ��5&      �� *NewTour.endLocation+$ PropertyChanged=�5'   <   �� 5)   
   $��Transport Type:��6   $��8��6+   $��5q�68   �� 5*   
   $��9��6   $��25��5+   H   $��5q�6T   ��5*       �� *TransportTypes+ �� SelectedValue 5+      �� *NewTour.transporttype+ �� Mode$ TwoWay=�5,      �� 5.   
   $��Distance (km):��6   $��10��6*   $��5q�68   �� 5/   
   $��11��6   $��Stretch=�50      $��25��60   $��5q�6<   ��5/       �� *NewTour.distance+$ PropertyChanged=�50   <   �� 52   
   $��Estimated Time (mins):��6   $��12��62   $��5q�6@   �� 53   
   $��13��6   $��Stretch=�54      $��25��60   $��5q�6<   ��53       �� *NewTour.estimatedTime+$ PropertyChanged=�54   <   �� 56   
   $
��Save��6   $��14��6    $��Center=�57      $��80��6.   $��30��69   $��10q�6E   ��56   .   �� *SaveTourCommand+57   E   58      59      