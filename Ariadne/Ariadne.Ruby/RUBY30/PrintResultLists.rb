$stdout.reopen(File.basename(__FILE__,".*")+".log", "w")
$stdout.sync = true

require "FeResPost"
include FeResPost

require "Utils"
 
# Creates and initializes the DataBase :
db=NastranDb.new()
db.Name="tmpDB"
db.readBdf("model.dat")

# Reading or generating Results :
db.readOp2("model.op2","Results")

# Inspecting Results :
db.each_resultKeyCaseId do |lcName|
    printf("LOADCASE: \"%s\"\n",lcName)
end
db.each_resultKeySubCaseId do |scName|
    printf("SUBCASE: \"%s\"\n",scName)
end
db.each_resultKeyLcScId do |lcName,scName|
    printf("LOADCASE and SUBCASE: \"%s\" - \"%s\"\n",lcName,scName)
end
db.each_resultKeyResId do |resName|
    printf("RESULT: \"%s\"\n",resName)
end

lcName="Load Case 1"
scName="Statics"
resName="Stress Tensor"
tmpRes=db.getResultCopy(lcName,scName,resName)
printf("%-20s%-25s%-60s%-10d\n",lcName,scName,resName,tmpRes.Size)

Util::printRes($stdout,resName,tmpRes)
