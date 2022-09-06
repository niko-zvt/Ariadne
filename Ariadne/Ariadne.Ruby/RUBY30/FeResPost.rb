# Copyright 2022 Renaud Sizaire
# Project: FeResPost (www.ferespost.eu)
# Licensed under the GNU Lesser General Public License version 3
# E-mail: ferespost@googlegroups.com

# encoding: utf-8
require "FeResPost.so"

module FeResPost
    class ROpResult
        @x=nil
        def initialize(res)
            @x=res
        end
        def +(other)
            return Post.opAdd(other,@x)
        end
        def -(other)
            return Post.opSub(other,@x)
        end
        def *(other)
            return Post.opMul(other,@x)
        end
        def /(other)
            return Post.opDiv(other,@x)
        end
        def **(other)
            return Post.pow(other,@x)
        end
    end
    class Result
        def coerce(x)
            [ROpResult.new(self),x]
        end
    end # Result
end # FeResPost 

class Array
	alias _FeFesPost_old_opAdd +
	alias _FeFesPost_old_opSub -
	alias _FeFesPost_old_opMul *
	def +(second)
		if second.class==Result then
			return Post.opAdd(self,second)
		else
			return self._FeFesPost_old_opAdd(second)
		end
	end
	def -(second)
		if second.class==Result then
			return Post.opSub(self,second)
		else
			return self._FeFesPost_old_opSub(second)
		end
	end
	def *(second)
		if second.class==Result then
			return Post.opMul(self,second)
		else
			return self._FeFesPost_old_opMul(second)
		end
	end
	def /(second)
		if second.class==Result then
			return Post.opDiv(self,second)
		else
			raise "Invalid second operand for / operator"
		end
	end
end # Array

puts "End \"FeResPost\" module initialization.\n"
puts "\n\n"
