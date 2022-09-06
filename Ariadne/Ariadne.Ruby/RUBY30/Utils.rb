# Copyright 2022 Renaud Sizaire
# Project: FeResPost (www.ferespost.eu)
# Licensed under the GNU Lesser General Public License version 3
# E-mail: ferespost@googlegroups.com

module Util

    def Util::printRes(os,name,res)
        os.printf("size of \"%s\" :  %d\n",name,res.Size)
        os.printf("format of \"%s\" :  %d\n",name,res.Format)
        os.printf("tensor order of \"%s\" :  %d\n",name,res.TensorOrder)
        os.printf("IntIds = %10d %10d\n",res.getIntId(0),res.getIntId(1))
        os.printf("RealIds = %14g %14g\n",res.getRealId(0),res.getRealId(1))
        res.each do |key,values|
            os.printf("        ")
            for j in 0..3
                if key[j] then
                    if (key[j].class==String) then
                        os.printf("%10s",key[j])
                    else
                        os.printf("%10s",key[j])
                    end
                else
                    os.printf("%10s","NONE")
                end
            end
            if values[0] then
                if (values[0].class==String) then
                    os.printf("%10s",values[0])
                else
                    os.printf("%10s",values[0])
                end
            else
                os.printf("%10s","NONE")
            end
            for j in 1...values.size
                if values[j] then
                    os.printf("%14g",values[j])
                end
            end
            os.printf("\n")
        end
    end
    
    def Util::printGrp(os,name,grp)
        os.printf("\n\n\n Group \"%s\" :\n\n",name)
        nodesNbr = grp.getEntitiesByType("Node").size
        elementsNbr = grp.getEntitiesByType("Element").size
        rbesNbr = grp.getEntitiesByType("MPC").size
        coordNbr = grp.getEntitiesByType("CoordSys").size
        os.printf("        nodesNbr = %d\n",nodesNbr)
        os.printf("        elementsNbr = %d\n",elementsNbr)
        os.printf("        rbesNbr = %d\n",rbesNbr)
        os.printf("        coordNbr = %d\n",coordNbr)
        os.printf("\n")
    end  
    
    def Util::printDbResList(os,db)
        db.each_resultKey do |lcName,scName,tpName|
            os.printf("%-20s%-25s%80s%10d\n",lcName,scName,tpName,\
                     db.getResultSize(lcName,scName,tpName))
        end
    end
    
    def Util::printDbResListB(os,db)
        db.each_resultKey do |lcName,scName,tpName|
            res=db.getResultCopy(lcName,scName,tpName)
            os.printf("%-20s%-25s%80s%10d%4d%4d\n",lcName,scName,tpName,\
                     res.Size,res.TensorOrder,res.Format)
        end
    end
    
    def Util::printDbGrpList(os,db)
        os.printf("%-30s%10s%10s%10s%10s\n","groupName","Nodes",\
            "Elements","RBEs","CoordSys")
        db.each_groupName do |groupName|
            grp = db.getGroupCopy(groupName)
            nodesNbr = grp.getEntitiesByType("Node").size
            elementsNbr = grp.getEntitiesByType("Element").size
            rbesNbr = grp.getEntitiesByType("MPC").size
            coordsNbr = grp.getEntitiesByType("CoordSys").size
            os.printf("%30s%10d%10d%10d%10d\n",groupName,nodesNbr,\
                elementsNbr,rbesNbr,coordsNbr)
        end
    end    
    
end
