/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package db;

import java.util.ArrayList;

/**
 *
 * @author jqin7
 */
public class TripletOperation {

    private String firstColumn;
    private String operator;
    private String literalOrSecondColumn;

    public TripletOperation(String fC, String op, String lOSC) {
        firstColumn = fC;
        operator = op;
        literalOrSecondColumn = lOSC;
    }

    public Column decipher(Table t) {
        if (literalOrSecondColumn == null) {
            return Table.getColFromString(firstColumn, t);
        }
        Column firstCol = Table.getColFromString(firstColumn, t);
        try {
            Integer intLiteral = Integer.parseInt(literalOrSecondColumn);
            if (firstCol.printType().equals("Integer")) {
                ArrayList<Integer> insert = this.callUnaryOperatorToMakeIntCol(firstCol, intLiteral);
                Column<Integer> newCol = new Column<>("anonymous", "Integer");
                newCol.singleColumn = insert;
                return newCol;
            }
            Float fltLiteral = Float.parseFloat(literalOrSecondColumn);
            ArrayList<Float> insert = this.callUnaryOperatorToMakeFltCol(firstCol, fltLiteral);
            Column<Float> newCol = new Column<>("anonymous", "Float");
            newCol.singleColumn = insert;
            return newCol;

        } catch (NumberFormatException nfe1) {
            try {
                Float floatLiteral = Float.parseFloat(literalOrSecondColumn);
                ArrayList<Float> insert = this.callUnaryOperatorToMakeFltCol(firstCol, floatLiteral);
                Column<Float> newCol = new Column<>("anonymous", "Float");
                newCol.singleColumn = insert;
                return newCol;
            } catch (NumberFormatException nfe2) {
                if (Database.isString(literalOrSecondColumn)) {
                    String SLit = literalOrSecondColumn;
                    if (operator.equals("+")) {
                        ArrayList<String> insert = firstCol.addUnaryS(SLit);
                        Column<String> newCol = new Column<>("anonymous", "String");
                        newCol.singleColumn = insert;
                        return newCol;
                    }
                    else {
                        throw new IllegalArgumentException("ERROR: You cannot use any operation other than + with Strings.");
                    }
                } else {
                    Column colComparison = Table.getColFromString(literalOrSecondColumn, t);
                    if (colComparison.printType().equals("Integer") && firstCol.printType().equals("Integer")) {
                        ArrayList<Integer> insert = this.callBinaryOperatorToMakeIntCol(firstCol, colComparison);
                        Column<Integer> newCol = new Column<>("anonymous", "Integer");
                        newCol.singleColumn = insert;
                        return newCol;
                    }
                    else if (colComparison.printType().equals("Float") || firstCol.printType().equals("Float")) {
                        ArrayList<Float> insert = this.callBinaryOperatorToMakeFltCol(firstCol, colComparison);
                        Column<Float> newCol = new Column<>("anonymous", "Float");
                        newCol.singleColumn = insert;
                        return newCol;
                    }
                    else {
                        if (operator.equals("+")) {
                            ArrayList<String> insert = firstCol.addBinaryS(colComparison);
                            Column<String> newCol = new Column<>("anonymous", "String");
                            newCol.singleColumn = insert;
                            return newCol;
                        }
                        else {
                            throw new IllegalArgumentException("ERROR: You cannot use any operation other than + with String Columns.");
                        }
                    }
                }
            }
        }
    }

    public ArrayList<Integer> callUnaryOperatorToMakeIntCol (Column col, Integer in) {
        if (operator.equals("+")) {
            return col.addUnaryI(in);
        }
        else if (operator.equals("-")) {
            return col.subtractUnaryI(in);
        }
        else if (operator.equals("*")) {
            return col.multiplyUnaryI(in);
        }
        else {
            return col.divideUnaryI(in);
        }
    }

    public ArrayList<Float> callUnaryOperatorToMakeFltCol (Column col, Float flt) {
        if (operator.equals("+")) {
            return col.addUnaryF(flt);
        }
        else if (operator.equals("-")) {
            return col.subtractUnaryF(flt);
        }
        else if (operator.equals("*")) {
            return col.multiplyUnaryF(flt);
        }
        else {
            return col.divideUnaryF(flt);
        }
    }

    public ArrayList<Integer> callBinaryOperatorToMakeIntCol (Column col, Column col2) {
        if (operator.equals("+")) {
            return col.addBinaryI(col2);
        }
        else if (operator.equals("-")) {
            return col.subtractBinaryI(col2);
        }
        else if (operator.equals("*")) {
            return col.multiplyBinaryI(col2);
        }
        else {
            return col.divideBinaryI(col2);
        }
    }

    public ArrayList<Float> callBinaryOperatorToMakeFltCol (Column col, Column col2) {
        if (operator.equals("+")) {
            return col.addBinaryF(col2);
        }
        else if (operator.equals("-")) {
            return col.subtractBinaryF(col2);
        }
        else if (operator.equals("*")) {
            return col.multiplyBinaryF(col2);
        }
        else {
            return col.divideBinaryF(col2);
        }
    }

}