   M S B A M L   `   `   `  ����   UserWallPaper%    UserWallPaper.NoUSBWindowsStyle   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  UserWindows�� � Title$ WindowsStyle�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=�$
��1680�� � ShowInTaskbar .  $
��1050�� � 	Resources    UserDock �� 
TargetType �� Property �� Value	 �� Triggers
 :� RoutedEvent �� TargetProperty	 W� To g� Duration  StandardBorderBrush �� Color  StandardBackgroundBrush  HoverBorderBrush  SelectedBackgroundBrush  SelectedForegroundBrush  DisabledForegroundBrush  NormalBrush  NormalBorderBrush 	 HorizontalNormalBrush 
 HorizontalNormalBorderBrush  ListBoxBackgroundBrush �� 
StartPoint �� EndPoint � Color � Offset  StandardBrush  
GlyphBrush  PressedBrush  ScrollBarLineButton �� 
TargetType �� CornerRadius �� Path �� RelativeSource �� Triggers P� Property P� Value �� 
TargetName  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBar � RowDefinitions ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton  HorizontalScrollBar � ColumnDefinitions  FavsScrollViewer  �� VerticalOffset! � Value" �� ScrollableHeight# � Maximum$ �� ViewportHeight% �� ViewportSize*& �� #ComputedVerticalScrollBarVisibility' �� Orientation( �� HorizontalOffset) �� ScrollableWidth* �� ViewportWidth,+ �� %ComputedHorizontalScrollBarVisibility��%^  &	        '��     &	 *    &	 :    &	 J    &	 Z    &	 j    &	 {    &	 �    &	 �    &		 �    &	
 �    &	 �    &	 D    &	 �    &	 �    &	 4    7  7  7  7  7  &	 �    &	 l    &	 4    7  7  7  7 	 7 
 7  7  &	 �    7  7  7  7  7  7  7  '�� �	    7  7  &	 �
    �� " �.���  � ��$ Center=���  �@�LayoutTransform �� 	 :� $
 UIElement.MouseEnter��.[��� .���� .��W� $ Opacity�$ 1X�$ 0:0:0.5D�:� $
 UIElement.MouseLeave��.[��� .���� .��W� $ Opacity�$ 0X�$ 0:0:3D��� " ��.���  � ��$ 10q��� $
 #888���� $
 #FFF���� $
 #DDD���� $
 Gray���� $ White���� $
 #888���� $
 #888���� $
 #888���� $
 #888���� $
 #888���� $	 0,0&�$ 1,0.001&���� � $ White��$	 0.0Q�� $ White��$	 0.6Q�� $ #DDDDDD��$	 1.2Q��� $	 0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #CCC��$	 1.0Q��� $
 #444���� $	 0,0&�$	 0,1&���� � $
 #BBB��$	 0.0Q�� $
 #EEE��$	 0.1Q�� $
 #EEE��$	 0.9Q�� $
 #FFF��$	 1.0Q��� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � |� .  ��  � �� �� " ��.R��� $��Border��$��1q�$ 2��8��  8�� $��1q�._�F� $��Center=�$��Center=�8�� ���� $ Content� �� *TemplatedParent+ P�  � �� . . ��� $ Border��8   � ��P�  � {� .  . ��� 8   � ���� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� .  ��  � |� .  ��  � �� �� " ��.R��� 
������ �� " n�.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� .  ��  � |� .  ��  � �� �� " n�.R��� $ 2��#��z��#��z��$��1q��� " ��.R��  �� $��18���� $��0.00001* ��� $��18��.+��� $��3��$ 2�����#F0F0F0�� $��0��8��  $��18��$��ScrollBar.LineUpCommand��$��M 0 4 L 8 4 L 4 0 Z��]� $��
PART_Track��$��1����.  �� 8�� $��ScrollBar.PageUpCommand�� n� 8�� $��1,0,1,0q�8�� 8��  �� 8�� $��ScrollBar.PageDownCommand���� $��3��8�� $��18��$��ScrollBar.LineDownCommand��$��M 0 0 L 4 4 L 8 0 Z���� " ��.R��  �� $��18���� $��0.00001* ��� $��18��.+��� $��3��$ 2�����#F0F0F0�� $��0��8��  $��18��$��ScrollBar.LineLeftCommand��$��M 4 0 L 4 8 L 0 4 Z��]� $��
PART_Track��$��1����.   �� 8�� $��ScrollBar.PageLeftCommand�� n� 8�� $��0,1,0,1q�8�� 8��  �� 8�� $ ��ScrollBar.PageRightCommand���� $��3��8�� $��18��$ ��ScrollBar.LineRightCommand��$��M 0 0 L 4 4 L 0 8 Z���� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . 	 P�  �@��Orientation$ 
Horizontal=�. ���  � ��$
 Auto����  � ��$ 18����  � ��8   P�  �@��Orientation$ Vertical=�. ���  � ��$ 18����  � ��$
 Auto����  � ��8  �� " ��.���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��  �� $
��Auto ���  �� �� $
��Auto �.+��� $��1���� $��PART_VerticalScrollBar��#! z  ## z" #% z$ #y�z& �� $��PART_HorizontalScrollBar��$' 
Horizontal=�$��1��$��1��#! z( ## z) #% z* #y�z+ .��� -   , �Name$, 
WallPaper1��- J� Opacity$- 0Q�.+��� -   . ��Name$. 
MainCanvas��$
��Left=�$
��1050��$	��Top=�$
��1680��.+��� -   / ��Name$/ 	Wallpaper��$
��Fill=�$
��1050��$
��1680��$��Center=�$��Center=�$��ImageFolder/wallpaper.jpg���� $
��Auto=�$��Disabled=�$��
0,127,0,66q�$
��Left=�$
��1680��.���� -   $. WindowCanvas��$
��1680��.+��� ���	#FFF4F4F5$	��923��$��0q�$
��1680��0 J� OpacityMask0 �� $	 1,1&�$	 1,1&�.��� $ 0Q�$ Black��� $ 0Q�$ Transparent��� $	��690��$	��514��1 � Orientation$1 
Horizontal=�2 �� Left$2 33��
3 �� Top$3 72��.+�� -   $	��300��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��300��$	��150��4 �� RadiusX$4 15��5 �� RadiusY$5 15��$��0,0,0,0q�$��10�����	#FF5AC8D3$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$ ��ImageFolder/MyComputer.png��$��	15,25,0,0q��� $
��Left=�$
��Wrap=�$��내 컴퓨터��$	��Top=�$��
135,20,0,0q�$��38��$	��157��$��30(�$��Vrinda)�
��������� $
��Left=�$
��Wrap=�$	��Top=�$��
133,65,0,0q�$��49��$	��167��$��15(�$��Vrinda)�
��������� $#�사용하시는 컴퓨터의 ���� �� $�컴퓨터를 엽니다.��� -   $	��150��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��150��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10�����	#FF1A83CD$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$ ��ImageFolder/SubMission.png��$��	25,15,0,0q��� $
��Left=�$
��Wrap=�$	��Top=�$��
13,123,0,0q�$��20��$	��130��$��15(�$��Vrinda)�
��������� $�과제 게시판��� -   $	��300��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��300��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10�����	#FFFF7E26$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$��ImageFolder/MyFolder.png��$��	15,25,0,0q��� $
��Left=�$
��Wrap=�$��
내 문서��$	��Top=�$��
135,20,0,0q�$��38��$	��135��$��30(�$��Vrinda)�
��������� $
��Left=�$
��Wrap=�$	��Top=�$��
133,65,0,0q�$��49��$	��167��$��15(�$��Vrinda)�
��������� $#�사용하시는 컴퓨터의 ���� �� $�내 문서를 엽니다.��� -	   $	��150��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��150��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10�����	#FF2BAC2B$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$"��ImageFolder/StudentBoard.png��$��	25,15,0,0q��� $
��Left=�$
��Wrap=�$	��Top=�$��
13,123,0,0q�$��20��$	��130��$��15(�$��Vrinda)�
��������� $�학생 게시판��� -
   $	��300��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��300��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10�����	#FFCD3333$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$��ImageFolder/Viewer.png��$��	15,25,0,0q��� $
��Left=�$
��Wrap=�$��사용자 교재��$	��Top=�$��
135,20,0,0q�$��38��$	��167��$��29(�$��Vrinda)�
��������� $
��Left=�$
��Wrap=�$	��Top=�$��
133,65,0,0q�$��49��$	��167��$��15(�$��Vrinda)�
��������� $#�사용자의 교재를 실행���� �� $�
합니다.��� -   $	��150��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��150��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10��
���2͚�$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$��ImageFolder/TrashCan.png��$��	25,15,0,0q��� $
��Left=�$
��Wrap=�$	��Top=�$��
13,123,0,0q�$��20��$	��130��$��15(�$��Vrinda)�
��������� $�	휴지통��� -   $	��300��$	��150��$��10q�$��Center=�$��Center=�
������ .+��� $	��300��$	��150��$4 15��$5 15��$��0,0,0,0q�$��10�����	#FF692FB0$
- 0.85Q��� $
��Left=�$	��100��$	��Top=�$	��100��$��ImageFolder/FileRoom.png��$��	15,25,0,0q��� $
��Left=�$
��Wrap=�$��	자료실��$	��Top=�$��
135,20,0,0q�$��38��$	��167��$��29(�$��Vrinda)�
��������� $
��Left=�$
��Wrap=�$	��Top=�$��
133,65,0,0q�$��49��$	��167��$��14(�$��Vrinda)�
��������� $'�!새로운 파일이 없습니다.��� -   6 �Name$6 wallpaperline1��$	��690��$
��1062��$1 Vertical=�$
��Left=�$	2 589��$3 72��7 J� 	AllowDrop7 . � .+��� -   8 ��Name$8 MainName��$
��Wrap=�$	��Top=�$��
0,72,159,0q�
�������$��40(�$��Right=�$��Vrinda)��� $
��Left=�$��50��$��1548,70,0,0q�$	��Top=�$��50��
��������� $
��Left=�$��40��$	��Top=�$��40��$��1553,73,0,0q�$��ImageFolder/User.png���� $
��Left=�$��48��$��Center=�$	��250�����	#FF2E2E2E$5 22��$4 22��$��1353,72,0,930q�9 � ContextMenu9 �� .@�m� -   $��종료��: m� Icon: �� 
�������; �� Padding$; 2q�._��� $��ImageFolder/End.png��$��25��$��25��0 �� $	 1,1&�$	 1,1&�.��� $ 0Q�$ Black��� $ 0Q�$ Transparent��� -   $, ShowLinkDock��$
��Left=�$
��1050��$	��Top=�$	��100��$��1612,0,-32,0q�.+��� $
��Left=�$��Center=�$
��1050��$	��100�����	#FF2E2E2E$5 0��$4 0��$��0q�0 �� $	 1,1&�$	 1,1&�.��� $	 0.5Q�$ Black��� $ 1Q�$ Transparent��