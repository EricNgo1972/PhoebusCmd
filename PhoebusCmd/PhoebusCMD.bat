
echo -------------
echo sender email is determined by order :  1- MCD profile, 2 - Operator ID, 3 - fixed phoebusERP@spc-technology.com 
c:
cd \

cd "Program Files (x86)\SPC Technology\PhoebusCMD"

rem PhoebusCmd u=ADM p=123456 d=TES mail=mailprofile c=S

rem PhoebusCmd u=userid p=pass d=PhoebusEntity sms=smsprofile c=S

rem PhoebusCmd u=userid p=pass d=PhoebusEntity pm=PM_Profile s=30

rem PhoebusCmd u=userid p=pass d=PhoebusEntity c=phoebusURL

rem phoebusURL example : "pbs.BO.WF.SendDueReminders?LineNo=<<1..99&RmType=BIRTHDAY"