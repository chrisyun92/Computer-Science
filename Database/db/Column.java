package db;

/**
 * @author jqin7 and Chris...
 */

import java.util.ArrayList;

/**
 * Created by Chris on 2/18/2017.
 */
public class Column<Body> {
    public ArrayList<Body> singleColumn;
    public String colName;
    private String bodyType;


    public Column(String name, String type) {

        bodyType = type;
        if (bodyType.equals("int")) {
            bodyType = "Integer";
        }
        else if (bodyType.equals("float")) {
            bodyType = "Float";
        }
        else if (bodyType.equals("string")){
            bodyType = "String";
        }
        colName = name;
        singleColumn = new ArrayList<Body>();
    }

    public static boolean Equal(Float d, Integer i) {
        if (d > i || d < i) {
            return false;
        }
        return true;
    }

    public static PairsOfIndices twoColumnCommonRows(Column c1, Column c2) {
        if (!c1.colName.equals(c2.colName) || !c1.bodyType.equals(c2.bodyType)) {
            throw new IllegalArgumentException("ERROR: Columns must have same name and data type.");
        }
        ArrayList<Integer> c1Indices = new ArrayList<Integer>();
        ArrayList<Integer> c2Indices = new ArrayList<Integer>();
        boolean canAddToc1I = false;
        int c1Index = 0;
        int c2Index = 0;
        ArrayList objectsToCheck = getBoth(c1.singleColumn, c2.singleColumn);
        for (Object entry : objectsToCheck) {
            for (Object c1obj : c1.singleColumn) {
                if (c1obj.equals(entry)) {
                    c1Indices.add(c1Index);
                }
                c1Index++;
            }
            for (Object c2obj : c2.singleColumn) {
                if (c2obj.equals(entry)) {
                    c2Indices.add(c2Index);
                }
                c2Index++;
            }
            c1Index = 0;
            c2Index = 0;

        }
        return new PairsOfIndices(c1Indices, c2Indices);

    }

    public static ArrayList getBoth(ArrayList a1, ArrayList a2) {
        ArrayList entities = new ArrayList();
        for (Object o : a1) {
            if (InsideBoth(a1, a2, o)) {
                if (!Inside(entities, o)) {
                    entities.add(o);
                }
            }
        }
        return entities;
    }

    public static boolean InsideBoth(ArrayList arr1, ArrayList arr2, Object o) {
        if (Inside(arr1, o) && Inside(arr2, o)) {
            return true;
        }
        return false;
    }

    public static boolean Inside(ArrayList arr, Object o) {
        for (Object ob : arr) {
            if (ob.equals(o)) {
                return true;
            }
        }
        return false;
    }

    public String getType() {
        if (this.bodyType.equals("Integer")) {

            return "int";
        } else if (this.bodyType.equals("Float")) {

            return "float";
        } else {

            return "string";
        }
    }

    public void add(Body... bodies) {

        for (Body b : bodies) {
            if (b instanceof Integer) {
                if (bodyType.equals("String") || bodyType.equals("Float")) {
                    throw new IllegalArgumentException("ERROR: Can only append Integer to Integer Column.");
                }
                singleColumn.add(b);
            } else if (b instanceof Float) {
                if (bodyType.equals("String") || bodyType.equals("Integer")) {
                    throw new IllegalArgumentException("ERROR: Can only append Float to Float Column.");
                }
                singleColumn.add(b);
            } else {
                if (bodyType.equals("Float") || bodyType.equals("Integer")) {
                    throw new IllegalArgumentException("ERROR: Can only append String to String Column.");
                }
                singleColumn.add(b);
            }

        }
    }

    public String printName() {
        return colName;
    }

    public String printType() {
        return bodyType;
    }

    public ArrayList<Integer> LTEUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if ((Integer) b <= compareWith) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if ((Float) b <= compareWith) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> LTEUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) <= 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> GTEUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if ((Integer) b >= compareWith) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if ((Float) b >= compareWith) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> GTEUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) >= 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> NotEqualUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if (!Equal(compareWith, (Integer) b)) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if (!((Float) b).equals(compareWith)) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> NotEqualUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) != 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> EqualUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if (Equal(compareWith, (Integer) b)) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if (((Float) b).equals(compareWith)) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> EqualUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) == 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> LesserUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if ((Integer) b < compareWith) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if ((Float) b < compareWith) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> LesserUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) < 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> GreaterUnary(Float compareWith) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                if ((Integer) b > compareWith) {
                    failed.add(index);
                }
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                if ((Float) b > compareWith) {
                    failed.add(index);
                }
                index++;
            }
        }
        return failed;

    }

    public ArrayList<Integer> GreaterUnary(String compareWith) {
        if (bodyType.equals("Float") || bodyType.equals("Integer")) {
            throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
        }
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            if (((String) b).compareTo(compareWith) > 0) {
                failed.add(index);
            }
            index++;
        }

        return failed;

    }

    public ArrayList<Integer> LTEBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b <= (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b <= (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b <= (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b <= (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) <= 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Integer> GTEBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b >= (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b >= (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b >= (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b >= (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) >= 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Integer> NotEqualBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if (!((Integer) b).equals((Integer)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if (!Equal((Float) b, (Integer)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if (!Equal((Float)compareWith.singleColumn.get(index), (Integer) b)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if (!((Float) b).equals((Float)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) != 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Integer> EqualBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if (((Integer) b).equals((Integer)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if (Equal((Float) b, (Integer)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if (Equal((Float)compareWith.singleColumn.get(index), (Integer) b)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if (((Float) b).equals((Float)compareWith.singleColumn.get(index))) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) == 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Integer> LesserBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b < (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b < (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b < (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b < (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) < 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Integer> GreaterBinary(Column compareWith) {
        ArrayList<Integer> failed = new ArrayList<Integer>();
        int index = 0;
        if (compareWith.bodyType.equals("Integer")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b > (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b > (Integer)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else if (compareWith.bodyType.equals("Float")) {
            if (bodyType.equals("String")) {
                throw new IllegalArgumentException("ERROR: Can't compare String with Number.");
            } else if (bodyType.equals("Integer")) {
                for (Body b : singleColumn) {
                    if ((Integer) b > (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            } else {
                for (Body b : singleColumn) {
                    if ((Float) b > (Float)compareWith.singleColumn.get(index)) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        } else {
            if (bodyType.equals("Integer") || bodyType.equals("Float")) {
                throw new IllegalArgumentException("ERROR: Can't compare Number with String.");
            } else {
                for (Body b : singleColumn) {
                    if (((String) b).compareTo((String)compareWith.singleColumn.get(index)) > 0) {
                        failed.add(index);
                    }
                    index++;
                }
            }
        }
        return failed;
    }

    public ArrayList<Float> addUnaryF(Float toAdd) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't add Float literal to String Column.");
        }
        ArrayList<Float> ret = new ArrayList<Float>();

        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b + toAdd);
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b + toAdd);
            }
        }
        return ret;
    }

    public ArrayList<Integer> addUnaryI(Integer toAdd) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY adds Integers to Integer Columns.");
        }
        ArrayList<Integer> ret = new ArrayList<Integer>();
        for (Body b : singleColumn) {
            ret.add((Integer) b + toAdd);
        }
        return ret;
    }

    public ArrayList<String> addUnaryS(String toAdd) {
        if (bodyType.equals("Integer") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY adds Strings to String Columns.");
        }
        ArrayList<String> ret = new ArrayList<String>();
        for (Body b : singleColumn) {
            ret.add(concat((String) b , toAdd));
        }
        return ret;
    }

    public ArrayList<Float> subtractUnaryF(Float toSubtract) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't subtract Float literal from String Column.");
        }
        ArrayList<Float> ret = new ArrayList<Float>();

        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b - toSubtract);
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b - toSubtract);
            }
        }
        return ret;
    }

    public ArrayList<Integer> subtractUnaryI(Integer toSubtract) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY subtracts Integers from Integer Columns.");
        }
        ArrayList<Integer> ret = new ArrayList<Integer>();
        for (Body b : singleColumn) {
            ret.add((Integer) b - toSubtract);
        }
        return ret;
    }

    public ArrayList<Float> multiplyUnaryF(Float toMultiply) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't multiply String Column by Float literal.");
        }
        ArrayList<Float> ret = new ArrayList<Float>();

        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b * toMultiply);
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b * toMultiply);
            }
        }
        return ret;
    }

    public ArrayList<Integer> multiplyUnaryI(Integer toMultiply) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY multiplies Integer Columns with Integers.");
        }
        ArrayList<Integer> ret = new ArrayList<Integer>();
        for (Body b : singleColumn) {
            ret.add((Integer) b * toMultiply);
        }
        return ret;
    }

    public ArrayList<Float> divideUnaryF(Float toDivide) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't multiply String Column by Float Literal.");
        }
        ArrayList<Float> ret = new ArrayList<Float>();

        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b / toDivide);
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b / toDivide);
            }
        }
        return ret;
    }

    public ArrayList<Integer> divideUnaryI(Integer toDivide) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY divides Integer Columns by Integers.");
        }
        ArrayList<Integer> ret = new ArrayList<Integer>();
        for (Body b : singleColumn) {
            ret.add((Integer) b / toDivide);
        }
        return ret;
    }

    public ArrayList<Float> addBinaryF(Column<Float> toAdd) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't add Float Column to String Column.");
        }
        ArrayList<Float> toUse = toAdd.singleColumn;
        ArrayList<Float> ret = new ArrayList<Float>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b + toUse.get(index));
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b + toUse.get(index));
                index++;
            }
        }
        return ret;

    }

    public ArrayList<Integer> addBinaryI(Column<Integer> toAdd) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY adds Integer Columns with Integer Columns.");
        }
        ArrayList<Integer> toUse = toAdd.singleColumn;
        ArrayList<Integer> ret = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            ret.add((Integer) b + toUse.get(index));
            index++;
        }
        return ret;

    }

    public ArrayList<String> addBinaryS(Column<String> toAdd) {
        if (bodyType.equals("Integer") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY adds String Columns with String Columns.");
        }
        ArrayList<String> toUse = toAdd.singleColumn;
        ArrayList<String> ret = new ArrayList<String>();
        int index = 0;
        for (Body b : singleColumn) {
            ret.add(concat((String) b, toUse.get(index)));
            index++;
        }
        return ret;
    }

    public ArrayList<Float> subtractBinaryF(Column<Float> toSubtract) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't subtract Float Column from String Column.");
        }
        ArrayList<Float> toUse = toSubtract.singleColumn;
        ArrayList<Float> ret = new ArrayList<Float>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b - toUse.get(index));
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b - toUse.get(index));
                index++;
            }
        }
        return ret;

    }

    public ArrayList<Integer> subtractBinaryI(Column<Integer> toSubtract) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY subtracts Integer Columns from Integer Columns.");
        }
        ArrayList<Integer> toUse = toSubtract.singleColumn;
        ArrayList<Integer> ret = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            ret.add((Integer) b - toUse.get(index));
            index++;
        }
        return ret;

    }

    public ArrayList<Float> multiplyBinaryF(Column<Float> toMultiply) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't multiply String Column with Float Column.");
        }
        ArrayList<Float> toUse = toMultiply.singleColumn;
        ArrayList<Float> ret = new ArrayList<Float>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b * toUse.get(index));
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b * toUse.get(index));
                index++;
            }
        }
        return ret;

    }

    public ArrayList<Integer> multiplyBinaryI(Column<Integer> toMultiply) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY multiplies Integer Columns with Integer Columns.");
        }
        ArrayList<Integer> toUse = toMultiply.singleColumn;
        ArrayList<Integer> ret = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            ret.add((Integer) b * toUse.get(index));
            index++;
        }
        return ret;

    }

    public ArrayList<Float> divideBinaryF(Column<Float> toDivide) {
        if (bodyType.equals("String")) {
            throw new IllegalArgumentException("ERROR: Can't divide String Column by Float Column.");
        }
        ArrayList<Float> toUse = toDivide.singleColumn;
        ArrayList<Float> ret = new ArrayList<Float>();
        int index = 0;
        if (bodyType.equals("Integer")) {
            for (Body b : singleColumn) {
                ret.add((Integer) b / toUse.get(index));
                index++;
            }
        } else {
            for (Body b : singleColumn) {
                ret.add((Float) b / toUse.get(index));
                index++;
            }
        }
        return ret;

    }

    public ArrayList<Integer> divideBinaryI(Column<Integer> toDivide) {
        if (bodyType.equals("String") || bodyType.equals("Float")) {
            throw new IllegalArgumentException("ERROR: This method ONLY divides Integer Columns by Integer Columns.");
        }
        ArrayList<Integer> toUse = toDivide.singleColumn;
        ArrayList<Integer> ret = new ArrayList<Integer>();
        int index = 0;
        for (Body b : singleColumn) {
            ret.add((Integer) b / toUse.get(index));
            index++;
        }
        return ret;

    }

    public int size() {
        return singleColumn.size();
    }

    public Body get(int x) {
        return singleColumn.get(x);
    }

    //This method will be used in order to obtain the singleColumns for Columns from indices designated by indicesToUseDuringJoins
    //This method will be used for two Tables during joins
    //Joins keeps the same colName and bodyType as before
    public Column<Body> obtainPartsOfColumnEntriesForJoins(ArrayList<Integer> indices) {
        ArrayList<Body> ret = new ArrayList<Body>();
        for (Integer index : indices) {
            ret.add(this.singleColumn.get(index));
        }
        Column<Body> joinpart = new Column<Body>(colName, bodyType);
        joinpart.singleColumn = ret;
        return joinpart;
    }

    public static String concat(String s1, String s2) {
        String ans = "";
        for (int a = 0; a < s1.length() - 1; a ++) {
            ans += s1.substring(a, a + 1);
        }
        for (int a = 1; a <s2.length(); a ++) {
            ans += s2.substring(a, a+1);
        }
        return ans;
    }

    public static ArrayList<Column> obtainAllColumnsWithTheseRowIndices (ArrayList<Column> alc, ArrayList<Integer> ali) {
        ArrayList<Column> ret = new ArrayList<>();
        for (Column c: alc) {
            ret.add(c.obtainPartsOfColumnEntriesForJoins(ali));
        }
        return ret;
    }

    public static ArrayList<Column> obtainPartsOfColumnsThatAreThisRow (ArrayList<Column> input, ArrayList<Column> unique, ArrayList row) {
        ArrayList<Integer> correctRows = new ArrayList<>();
        for (int index = 0; index < input.get(0).size(); index ++) {
            ArrayList thisRow = new ArrayList();
            for (Column c: input) {
                thisRow.add(c.get(index));
            }
            if (Table.equalArrayLists(thisRow, row)) {
                correctRows.add(index);
            }
        }
        return Column.obtainAllColumnsWithTheseRowIndices(unique, correctRows);
    }

    public static Column stackColumns (Column c1, Column c2) {
        Column ret = new Column(c1.printName(), c1.printType());
        ret.singleColumn.addAll(c1.singleColumn);
        ret.singleColumn.addAll(c2.singleColumn);
        return ret;
    }


    //remind self to obtain cartesian joins method from Chris
}